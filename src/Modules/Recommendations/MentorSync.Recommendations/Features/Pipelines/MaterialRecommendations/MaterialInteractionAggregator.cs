using MentorSync.Ratings.Contracts.Services;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Features.Pipelines.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;

public sealed class MaterialInteractionAggregator(
    RecommendationsDbContext db,
    IMaterialReviewService materialReviewService,
    ILogger<MaterialInteractionAggregator> logger)
        : IInteractionAggregator
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Running ETL for learning materials...");

            var viewEvents = await db.MaterialViewEvents.ToListAsync(cancellationToken);
            logger.LogInformation("Loaded {Count} material view events", viewEvents.Count);

            var likes = await db.MaterialLikes.ToListAsync(cancellationToken);
            logger.LogInformation("Loaded {Count} material likes", likes.Count);

            var materialRatings = await materialReviewService.GetAllReviewsAsync(cancellationToken);
            logger.LogInformation("Loaded {Count} material ratings", materialRatings.Count);

            // Use a dictionary with mentee-material pair as key and score as value
            var interactionScores = new Dictionary<(int menteeId, int materialId), float>();

            // Add scores for different interaction types
            foreach (var view in viewEvents)
                AddScore((view.MenteeId, view.MaterialId), 1);

            foreach (var like in likes)
                AddScore((like.MenteeId, like.MaterialId), 2);

            // Add explicit ratings (if any exist)
            foreach (var rating in materialRatings)
                AddScore((rating.MenteeId, rating.MaterialId), rating.Rating);

            // Helper method to safely get or set a value
            void AddScore((int menteeId, int materialId) key, float scoreToAdd)
            {
                try
                {
                    if (interactionScores.ContainsKey(key))
                        interactionScores[key] += scoreToAdd;
                    else
                        interactionScores[key] = scoreToAdd;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing score for mentee {MenteeId} and material {MaterialId}", key.menteeId, key.materialId);
                }
            }

            // Update database with aggregated scores
            foreach (var kvp in interactionScores)
            {
                try
                {
                    var (menteeId, materialId) = kvp.Key;
                    var score = kvp.Value;

                    var existing = await db.MenteeMaterialInteractions
                        .FirstOrDefaultAsync(x => x.MenteeId == menteeId && x.MaterialId == materialId, cancellationToken);

                    if (existing != null)
                    {
                        existing.Score = score;
                        existing.UpdatedAt = DateTime.UtcNow;
                        logger.LogDebug("Updated interaction score for mentee {MenteeId} and material {MaterialId} to {Score}", menteeId, materialId, score);
                    }
                    else
                    {
                        db.MenteeMaterialInteractions.Add(new MenteeMaterialInteraction
                        {
                            MenteeId = menteeId,
                            MaterialId = materialId,
                            Score = score
                        });
                        logger.LogDebug("Created new interaction score for mentee {MenteeId} and material {MaterialId} with {Score}", menteeId, materialId, score);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating interaction score for pair ({MenteeId}, {MaterialId})", kvp.Key.menteeId, kvp.Key.materialId);
                    // Continue processing other records despite error
                }
            }

            try
            {
                var savedChanges = await db.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Learning materials ETL completed. Saved {Count} changes to the database", savedChanges);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save learning material interaction scores to the database");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the learning material interaction aggregation process");
            throw;
        }
    }
}

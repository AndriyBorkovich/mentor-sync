﻿using MentorSync.Ratings.Contracts.Services;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Scheduling.Contracts;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;

public sealed class MentorInteractionAggregator(
    RecommendationsDbContext db,
    IMentorReviewService mentorReviewService,
    IBookingService bookingService,
    ILogger<MentorInteractionAggregator> logger)
    : IInteractionAggregator
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running ETL...");

        var viewEvents = await db.MentorViewEvents.ToListAsync(cancellationToken);
        logger.LogInformation("Loaded {Count} mentor view events", viewEvents.Count);

        var bookmarks = await db.MentorBookmarks.ToListAsync(cancellationToken);
        logger.LogInformation("Loaded {Count} mentor bookmarks", bookmarks.Count);

        var mentorReviews = await mentorReviewService.GetAllReviewsAsync(cancellationToken);
        logger.LogInformation("Loaded {Count} mentor reviews", mentorReviews.Count);

        var bookings = await bookingService.GetAllBookingsAsync(cancellationToken);
        logger.LogInformation("Loaded {Count} bookings", bookings.Count);

        // Use a dictionary with default value of 0 for new keys
        var interactionScores = new Dictionary<(int menteeId, int mentorId), float>();

        foreach (var view in viewEvents)
            AddScore((view.MenteeId, view.MentorId), 1);

        foreach (var bookmark in bookmarks)
            AddScore((bookmark.MenteeId, bookmark.MentorId), 3);

        foreach (var review in mentorReviews)
        {
            AddScore((review.MenteeId, review.MentorId), review.Rating);
        }

        foreach (var booking in bookings)
        {
            var scoreChange = booking.Status switch
            {
                BookingStatus.Completed => 3,
                BookingStatus.Cancelled => -1,
                BookingStatus.NoShow => -2,
                _ => 0
            };

            AddScore((booking.MenteeId, booking.MentorId), scoreChange);
        }

        // Helper method to safely get or set a value
        void AddScore((int menteeId, int mentorId) key, float scoreToAdd)
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
                logger.LogError(ex, "Error processing score for mentee {MenteeId} and mentor {MentorId}", key.menteeId, key.mentorId);
            }
        }

        foreach (var kvp in interactionScores)
        {
            try
            {
                var (menteeId, mentorId) = kvp.Key;
                var score = kvp.Value;

                var existing = await db.MenteeMentorInteractions
                    .FirstOrDefaultAsync(x => x.MenteeId == menteeId && x.MentorId == mentorId, cancellationToken);

                if (existing != null)
                {
                    existing.Score = score;
                    existing.UpdatedAt = DateTime.UtcNow;
                    logger.LogDebug("Updated interaction score for mentee {MenteeId} and mentor {MentorId} to {Score}", menteeId, mentorId, score);
                }
                else
                {
                    db.MenteeMentorInteractions.Add(new MentorMenteeInteraction
                    {
                        MenteeId = menteeId,
                        MentorId = mentorId,
                        Score = score
                    });
                    logger.LogDebug("Created new interaction score for mentee {MenteeId} and mentor {MentorId} with {Score}", menteeId, mentorId, score);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating interaction score for pair ({MenteeId}, {MentorId})", kvp.Key.menteeId, kvp.Key.mentorId);
                // Continue processing other records despite error
            }
        }

        var savedChanges = await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("ETL completed. Saved {Count} changes to the database", savedChanges);
    }
}

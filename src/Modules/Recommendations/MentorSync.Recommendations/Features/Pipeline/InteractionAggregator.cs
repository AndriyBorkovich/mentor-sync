using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Interaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.Pipeline;

/// <summary>
/// collects and scores events (ETL)
/// </summary>
public interface IInteractionAggregator
{
    Task RunAsync(CancellationToken cancellationToken);
}

public sealed class InteractionAggregator(
    RecommendationDbContext db,
    ILogger<InteractionAggregator> logger)
    : IInteractionAggregator
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running ETL...");

        var viewEvents = await db.MentorViewEvents.ToListAsync(cancellationToken);
        var bookmarks = await db.MentorBookmarks.ToListAsync(cancellationToken);

        var interactionScores = new Dictionary<(int menteeId, int mentorId), float>();

        foreach (var view in viewEvents)
            interactionScores[(view.MenteeId, view.MentorId)] = 1;

        foreach (var bookmark in bookmarks)
            interactionScores[(bookmark.MenteeId, bookmark.MentorId)] += 3;

        foreach (var kvp in interactionScores)
        {
            var (menteeId, mentorId) = kvp.Key;
            var score = kvp.Value;

            var existing = await db.MenteeMentorInteractions
                .FirstOrDefaultAsync(x => x.MenteeId == menteeId && x.MentorId == mentorId, cancellationToken);

            if (existing != null)
            {
                existing.Score = score;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                db.MenteeMentorInteractions.Add(new MentorMenteeInteraction
                {
                    MenteeId = menteeId,
                    MentorId = mentorId,
                    Score = score
                });
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("ETL completed.");
    }
}



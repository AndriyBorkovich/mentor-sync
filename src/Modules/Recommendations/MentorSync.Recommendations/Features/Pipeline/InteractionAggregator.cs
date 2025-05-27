using MentorSync.Ratings.Contracts;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Scheduling.Contracts;
using MentorSync.SharedKernel.CommonEntities;
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
    RecommendationsDbContext db,
    IMentorReviewService mentorReviewService,
    IBookingService bookingService,
    ILogger<InteractionAggregator> logger)
    : IInteractionAggregator
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Running ETL...");

        var viewEvents = await db.MentorViewEvents.ToListAsync(cancellationToken);
        var bookmarks = await db.MentorBookmarks.ToListAsync(cancellationToken);
        var mentorReviews = await mentorReviewService.GetAllReviewsAsync(cancellationToken);
        var bookings = await bookingService.GetAllBookingsAsync(cancellationToken);

        var interactionScores = new Dictionary<(int menteeId, int mentorId), float>();

        foreach (var view in viewEvents)
            interactionScores[(view.MenteeId, view.MentorId)] = 1;

        foreach (var bookmark in bookmarks)
            interactionScores[(bookmark.MenteeId, bookmark.MentorId)] += 3;

        foreach (var review in mentorReviews)
            interactionScores[(review.MenteeId, review.MentorId)] += review.Rating;

        foreach (var booking in bookings)
        {
            if (booking.Status == BookingStatus.Completed)
            {
                interactionScores[(booking.MenteeId, booking.MentorId)] += 3;
            }
            else if (booking.Status == BookingStatus.Cancelled)
            {
                interactionScores[(booking.MenteeId, booking.MentorId)] -= 1;
            }
            else if (booking.Status == BookingStatus.NoShow)
            {
                interactionScores[(booking.MenteeId, booking.MentorId)] -= 2;
            }
        }

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



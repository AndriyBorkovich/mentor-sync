using MentorSync.Ratings.Contracts.Models;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Domain.Tracking;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Scheduling.Contracts;
using MentorSync.Scheduling.Contracts.Models;
using MentorSync.SharedKernel.CommonEntities.Enums;
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

		var interactionScores = new Dictionary<(int menteeId, int mentorId), float>();

		AddScores(viewEvents, bookmarks, mentorReviews, bookings, interactionScores);

		var savedChanges = await ProcessScoresAsync(interactionScores, cancellationToken);
		logger.LogInformation("ETL completed. Saved {Count} changes to the database", savedChanges);
	}

	private void AddScores(List<MentorViewEvent> viewEvents,
		List<MentorBookmark> bookmarks,
		IReadOnlyList<MentorReviewResult> mentorReviews,
		IReadOnlyList<BookingModel> bookings,
		Dictionary<(int menteeId, int mentorId), float> interactionScores)
	{
		foreach (var view in viewEvents)
		{
			AddScore((view.MenteeId, view.MentorId), 1);
		}

		foreach (var bookmark in bookmarks)
		{
			AddScore((bookmark.MenteeId, bookmark.MentorId), 3);
		}

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
				_ => 0,
			};

			AddScore((booking.MenteeId, booking.MentorId), scoreChange);
		}

		return;

		void AddScore((int menteeId, int mentorId) key, float scoreToAdd)
		{
			try
			{
				if (!interactionScores.TryAdd(key, scoreToAdd))
				{
					interactionScores[key] += scoreToAdd;
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error processing score for mentee {MenteeId} and mentor {MentorId}", key.menteeId, key.mentorId);
			}
		}
	}

	private async Task<int> ProcessScoresAsync(Dictionary<(int menteeId, int mentorId), float> interactionScores, CancellationToken cancellationToken)
	{
		foreach (var (key, score) in interactionScores)
		{
			try
			{
				var (menteeId, mentorId) = key;

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
				logger.LogError(ex, "Error updating interaction score for pair ({MenteeId}, {MentorId})", key.menteeId, key.mentorId);
			}
		}

		var savedChanges = await db.SaveChangesAsync(cancellationToken);
		return savedChanges;
	}
}

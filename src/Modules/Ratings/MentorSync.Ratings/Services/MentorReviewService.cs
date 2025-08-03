using MentorSync.Ratings.Contracts.Models;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Services;

internal sealed class MentorReviewService(RatingsDbContext ratingsDbContext) : IMentorReviewService
{
	public double GetAverageRating(int mentorId)
	{
		return ratingsDbContext.MentorReviews
			.AsNoTracking()
			.Where(review => review.MentorId == mentorId)
			.Average(review => review.Rating);
	}

	public async Task<IReadOnlyList<MentorReviewResult>> GetReviewsByMentorAsync(int mentorId)
	{
		return await ratingsDbContext.MentorReviews
			.AsNoTracking()
			.Where(review => review.MentorId == mentorId)
			.Select(review => new MentorReviewResult
			{
				MentorId = review.MentorId,
				MenteeId = review.MenteeId,
				Rating = review.Rating,
			})
			.ToListAsync();
	}

	public async Task<IReadOnlyList<MentorReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
	{
		return await ratingsDbContext.MentorReviews
			.AsNoTracking()
			.Select(review => new MentorReviewResult
			{
				MentorId = review.MentorId,
				MenteeId = review.MenteeId,
				Rating = review.Rating,
			})
			.ToListAsync(cancellationToken);
	}
}

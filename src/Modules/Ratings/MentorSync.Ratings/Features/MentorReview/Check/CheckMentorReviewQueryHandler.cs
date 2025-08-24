using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MentorReview.Check;

public sealed class CheckMentorReviewQueryHandler(
	RatingsDbContext dbContext)
	: IQueryHandler<CheckMentorReviewQuery, CheckMentorReviewResponse>
{
	public async Task<Result<CheckMentorReviewResponse>> Handle(
		CheckMentorReviewQuery query,
		CancellationToken cancellationToken = default)
	{
		var existingReview = await dbContext.MentorReviews
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.MentorId == query.MentorId && r.MenteeId == query.MenteeId,
					cancellationToken);

		var response = new CheckMentorReviewResponse
		{
			HasReviewed = existingReview != null,
			ReviewId = existingReview?.Id,
			Rating = existingReview?.Rating,
			ReviewText = existingReview?.ReviewText,
		};

		return Result.Success(response);
	}
}

using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MentorReview.Delete;

public sealed class DeleteMentorReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<DeleteMentorReviewCommand, string>
{
	public async Task<Result<string>> Handle(
		DeleteMentorReviewCommand command,
		CancellationToken cancellationToken = default)
	{
		// Find the existing review
		var review = await dbContext.MentorReviews
			.FirstOrDefaultAsync(r => r.Id == command.ReviewId, cancellationToken);

		if (review == null)
		{
			return Result.NotFound("Review not found");
		}

		// Check if the review belongs to this mentee
		if (review.MenteeId != command.MenteeId)
		{
			return Result.Forbidden("You can only delete your own reviews");
		}

		// Remove the review
		dbContext.MentorReviews.Remove(review);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success("Review deleted successfully");
	}
}

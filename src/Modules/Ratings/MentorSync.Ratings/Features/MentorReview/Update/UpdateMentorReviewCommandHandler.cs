using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MentorReview.Update;

public sealed class UpdateMentorReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<UpdateMentorReviewCommand, UpdateMentorReviewResponse>
{
	public async Task<Result<UpdateMentorReviewResponse>> Handle(
		UpdateMentorReviewCommand command,
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
			return Result.Forbidden("You can only update your own reviews");
		}

		// Update the review
		review.Rating = command.Rating;
		review.ReviewText = command.ReviewText;
		review.UpdatedAt = DateTime.UtcNow;

		await dbContext.SaveChangesAsync(cancellationToken);

		var response = new UpdateMentorReviewResponse
		{
			ReviewId = review.Id,
			MentorId = review.MentorId,
			MenteeId = review.MenteeId,
			Rating = review.Rating,
			ReviewText = review.ReviewText,
			UpdatedAt = review.UpdatedAt ?? DateTime.UtcNow,
		};

		return Result.Success(response);
	}
}

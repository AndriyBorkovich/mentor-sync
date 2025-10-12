using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MaterialReview.Delete;

/// <summary>
/// Handler for deleting a material review
/// </summary>
/// <param name="dbContext"></param>
public sealed class DeleteMaterialReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<DeleteMaterialReviewCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(DeleteMaterialReviewCommand request, CancellationToken cancellationToken = default)
	{
		var review = await dbContext.MaterialReviews
							.FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

		if (review is null)
		{
			return Result.NotFound($"Review with ID {request.ReviewId} not found");
		}

		// Check if the user is the author of the review
		if (review.ReviewerId != request.UserId)
		{
			return Result.Forbidden("You can only delete your own reviews");
		}

		dbContext.MaterialReviews.Remove(review);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success("Review deleted successfully");
	}
}

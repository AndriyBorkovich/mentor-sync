using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MaterialReview.Update;

/// <summary>
/// Command to update a material review
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class UpdateMaterialReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<UpdateMaterialReviewCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(UpdateMaterialReviewCommand request, CancellationToken cancellationToken = default)
	{
		var review = await dbContext.MaterialReviews
				 .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

		if (review == null)
		{
			return Result.NotFound($"Review with ID {request.ReviewId} not found");
		}

		if (review.ReviewerId != request.ReviewerId)
		{
			return Result.Forbidden("You can only update your own reviews");
		}

		// Update the review
		review.Rating = request.Rating;
		review.ReviewText = request.ReviewText;
		review.UpdatedAt = DateTime.UtcNow;

		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success("Review updated successfully");
	}
}

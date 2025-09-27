using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MaterialReview.Create;

public sealed class CreateMaterialReviewCommandHandler(
	RatingsDbContext dbContext)
	: ICommandHandler<CreateMaterialReviewCommand, CreateMaterialReviewResponse>
{
	public async Task<Result<CreateMaterialReviewResponse>> Handle(
		CreateMaterialReviewCommand request, CancellationToken cancellationToken = default)
	{
		var materialId = request.MaterialId;
		var materialExists = await dbContext.MaterialReviews
			.AsNoTracking()
			.AnyAsync(r => r.MaterialId == materialId, cancellationToken);

		if (!materialExists)
		{
			return Result.NotFound($"Material with ID {materialId} does not exist.");
		}

		// Check if the user has already reviewed this material
		var existingReview = await dbContext.MaterialReviews
			.FirstOrDefaultAsync(r => r.MaterialId == request.MaterialId && r.ReviewerId == request.ReviewerId, cancellationToken);

		if (existingReview != null)
		{
			return Result.Error("You have already submitted a review for this material");
		}

		// Create and save the review
		var review = new Domain.MaterialReview
		{
			MaterialId = request.MaterialId,
			ReviewerId = request.ReviewerId,
			Rating = request.Rating,
			ReviewText = request.ReviewText,
			CreatedAt = DateTime.UtcNow,
		};

		await dbContext.MaterialReviews.AddAsync(review, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success(new CreateMaterialReviewResponse(review.Id));
	}
}

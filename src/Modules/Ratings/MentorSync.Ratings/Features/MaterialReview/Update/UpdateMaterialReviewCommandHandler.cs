using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.MaterialReview.Update;

public sealed class UpdateMaterialReviewCommandHandler(
    RatingsDbContext dbContext,
    ILogger<UpdateMaterialReviewCommandHandler> logger)
    : IRequestHandler<UpdateMaterialReviewCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateMaterialReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var review = await dbContext.MaterialReviews
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

            if (review == null)
            {
                return Result.NotFound($"Review with ID {request.ReviewId} not found");
            }

            // Ensure the user can only update their own reviews
            if (review.ReviewerId != request.ReviewerId)
            {
                return Result.Forbidden("You can only update your own reviews");
            }

            // Update the review
            review.Rating = request.Rating;
            review.ReviewText = request.ReviewText;
            review.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating material review: {Message}", ex.Message);
            return Result.Error($"An error occurred while updating the material review: {ex.Message}");
        }
    }
}

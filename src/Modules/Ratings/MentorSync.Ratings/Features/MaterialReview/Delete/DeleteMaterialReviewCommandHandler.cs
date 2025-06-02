using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.MaterialReview.Delete;

public sealed class DeleteMaterialReviewCommandHandler(
    RatingsDbContext dbContext,
    ILogger<DeleteMaterialReviewCommandHandler> logger)
    : IRequestHandler<DeleteMaterialReviewCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteMaterialReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var review = await dbContext.MaterialReviews
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

            if (review == null)
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

            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting material review: {Message}", ex.Message);
            return Result.Error($"An error occurred while deleting the material review: {ex.Message}");
        }
    }
}

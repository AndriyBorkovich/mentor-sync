using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.UpdateMentorReview;

public sealed class UpdateMentorReviewCommandHandler(
    RatingsDbContext dbContext,
    ILogger<UpdateMentorReviewCommandHandler> logger)
    : IRequestHandler<UpdateMentorReviewCommand, Result<UpdateMentorReviewResponse>>
{
    public async Task<Result<UpdateMentorReviewResponse>> Handle(
        UpdateMentorReviewCommand command,
        CancellationToken cancellationToken)
    {
        try
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
                UpdatedAt = review.UpdatedAt ?? DateTime.UtcNow
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating mentor review {ReviewId} by mentee {MenteeId}",
                command.ReviewId, command.MenteeId);

            return Result.Error($"Failed to update review: {ex.Message}");
        }
    }
}

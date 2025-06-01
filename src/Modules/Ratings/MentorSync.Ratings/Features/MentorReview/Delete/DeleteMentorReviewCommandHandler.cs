using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace MentorSync.Ratings.Features.MentorReview.Delete;

public sealed class DeleteMentorReviewCommandHandler(
    RatingsDbContext dbContext,
    ILogger<DeleteMentorReviewCommandHandler> logger)
    : IRequestHandler<DeleteMentorReviewCommand, Result>
{
    public async Task<Result> Handle(
        DeleteMentorReviewCommand command,
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
                return Result.Forbidden("You can only delete your own reviews");
            }

            // Remove the review
            dbContext.MentorReviews.Remove(review);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting mentor review {ReviewId} by mentee {MenteeId}",
                command.ReviewId, command.MenteeId);

            return Result.Error($"Failed to delete review: {ex.Message}");
        }
    }
}

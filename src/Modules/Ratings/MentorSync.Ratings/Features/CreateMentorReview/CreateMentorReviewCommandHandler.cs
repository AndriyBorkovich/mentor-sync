using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using MentorSync.Ratings.Domain;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.CreateMentorReview;

public sealed class CreateMentorReviewCommandHandler(
    RatingsDbContext dbContext,
    ILogger<CreateMentorReviewCommandHandler> logger)
    : IRequestHandler<CreateMentorReviewCommand, Result<CreateMentorReviewResponse>>
{
    public async Task<Result<CreateMentorReviewResponse>> Handle(
        CreateMentorReviewCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if the mentee has already reviewed this mentor
            var existingReview = await dbContext.MentorReviews
                .FirstOrDefaultAsync(r => r.MentorId == command.MentorId && r.MenteeId == command.MenteeId,
                    cancellationToken);

            if (existingReview != null)
            {
                return Result.Conflict("You have already submitted a review for this mentor");
            }

            var review = new MentorReview
            {
                MentorId = command.MentorId,
                MenteeId = command.MenteeId,
                Rating = command.Rating,
                ReviewText = command.ReviewText,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.MentorReviews.Add(review);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new CreateMentorReviewResponse
            {
                ReviewId = review.Id,
                MentorId = review.MentorId,
                MenteeId = review.MenteeId,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                CreatedAt = review.CreatedAt
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating mentor review for mentor {MentorId} by mentee {MenteeId}",
                command.MentorId, command.MenteeId);

            return Result.Error($"Failed to create review: {ex.Message}");
        }
    }
}

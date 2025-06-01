using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.MentorReview.Check;

public sealed class CheckMentorReviewQueryHandler(
    RatingsDbContext dbContext,
    ILogger<CheckMentorReviewQueryHandler> logger)
    : IRequestHandler<CheckMentorReviewQuery, Result<CheckMentorReviewResponse>>
{
    public async Task<Result<CheckMentorReviewResponse>> Handle(
        CheckMentorReviewQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingReview = await dbContext.MentorReviews
                .FirstOrDefaultAsync(r => r.MentorId == query.MentorId && r.MenteeId == query.MenteeId,
                    cancellationToken);

            var response = new CheckMentorReviewResponse
            {
                HasReviewed = existingReview != null,
                ReviewId = existingReview?.Id,
                Rating = existingReview?.Rating,
                ReviewText = existingReview?.ReviewText
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if mentee {MenteeId} has reviewed mentor {MentorId}",
                query.MenteeId, query.MentorId);

            return Result.Error($"Failed to check review: {ex.Message}");
        }
    }
}

using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.GetMentorReviews;

public class GetMentorReviewsQueryHandler(RatingsDbContext dbContext, ILogger<GetMentorReviewsQueryHandler> logger) : IRequestHandler<GetMentorReviewsQuery, Result<MentorReviewsResponse>>
{
    public async Task<Result<MentorReviewsResponse>> Handle(GetMentorReviewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the reviews and reviewer information using LINQ instead of raw SQL
            var reviews = await dbContext.MentorReviews
                .Where(mr => mr.MentorId == request.MentorId)
                .OrderByDescending(mr => mr.CreatedAt)
                .Select(review => new MentorReview
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.ReviewText,
                    CreatedOn = review.CreatedAt
                })
                .Take(10)
                .ToListAsync(cancellationToken);

            var reviewCount = await dbContext.MentorReviews
                .Where(mr => mr.MentorId == request.MentorId)
                .CountAsync(cancellationToken);

            var response = new MentorReviewsResponse
            {
                ReviewCount = reviewCount,
                Reviews = reviews
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting mentor reviews for MentorId: {MentorId}", request.MentorId);
            return Result.Error($"An error occurred while getting mentor reviews: {ex.Message}");
        }
    }
}

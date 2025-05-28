using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MentorSync.Ratings.Features.GetMentorReviews;

public class GetMentorReviewsQueryHandler(
    RatingsDbContext dbContext,
    ILogger<GetMentorReviewsQueryHandler> logger)
    : IRequestHandler<GetMentorReviewsQuery, Result<MentorReviewsResponse>>
{
    public async Task<Result<MentorReviewsResponse>> Handle(GetMentorReviewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the reviews
            var reviews = await dbContext.MentorReviews
                .Where(mr => mr.MentorId == request.MentorId)
                .OrderByDescending(mr => mr.CreatedAt)
                .Select(review => new MentorReview
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.ReviewText,
                    CreatedOn = review.CreatedAt,
                    ReviewerName = $"Mentee {review.MenteeId}", // This would be replaced with actual mentee name
                    ReviewerImage = "/assets/avatars/default.jpg" // Default image
                })
                .Take(20)
                .ToListAsync(cancellationToken);

            // Get the total review count
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

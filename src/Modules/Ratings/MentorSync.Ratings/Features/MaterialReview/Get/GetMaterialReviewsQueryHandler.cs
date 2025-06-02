using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MaterialReviewEntityDto = MentorSync.Ratings.Features.MaterialReview.Get.MaterialReview;

namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class GetMaterialReviewsQueryHandler(
    RatingsDbContext dbContext,
    ILogger<GetMaterialReviewsQueryHandler> logger)
    : IRequestHandler<GetMaterialReviewsQuery, Result<MaterialReviewsResponse>>
{
    public async Task<Result<MaterialReviewsResponse>> Handle(GetMaterialReviewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the reviews
            var reviews = await dbContext.MaterialReviews
                .Where(mr => mr.MaterialId == request.MaterialId)
                .OrderByDescending(mr => mr.CreatedAt)
                .Select(review => new MaterialReviewEntityDto
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    Comment = review.ReviewText,
                    CreatedOn = review.CreatedAt,
                    ReviewerName = $"User {review.ReviewerId}", // This would be replaced with actual user name
                    ReviewerImage = "/assets/avatars/default.jpg", // Default image
                    IsReviewByMentor = false // This would need to be determined by checking the user role
                })
                .Take(50)
                .ToListAsync(cancellationToken);

            // Get the review count and average rating
            var stats = await dbContext.MaterialReviews
                .Where(mr => mr.MaterialId == request.MaterialId)
                .GroupBy(x => true) // Group all reviews together
                .Select(g => new
                {
                    Count = g.Count(),
                    Average = g.Average(r => r.Rating)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var response = new MaterialReviewsResponse
            {
                ReviewCount = stats?.Count ?? 0,
                AverageRating = stats?.Average ?? 0,
                Reviews = reviews
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting material reviews for MaterialId: {MaterialId}", request.MaterialId);
            return Result.Error($"An error occurred while getting material reviews: {ex.Message}");
        }
    }
}

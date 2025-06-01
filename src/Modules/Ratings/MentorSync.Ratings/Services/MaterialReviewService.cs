using MentorSync.Ratings.Contracts.Models;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Services;

internal sealed class MaterialReviewService(RatingsDbContext dbContext) : IMaterialReviewService
{
    public async Task<List<MaterialReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
    {
        var ratings = await dbContext.MaterialReviews
            .Select(r => new MaterialReviewResult
            {
                MenteeId = r.ReviewerId,
                MaterialId = r.MaterialId,
                Rating = r.Rating,
            })
            .ToListAsync(cancellationToken);

        return ratings;
    }
}

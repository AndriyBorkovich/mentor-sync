using MentorSync.Ratings.Contracts.Models;
using MentorSync.Ratings.Contracts.Services;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Services;

internal sealed class MaterialReviewService(RatingsDbContext dbContext) : IMaterialReviewService
{
	public async Task<IReadOnlyList<MaterialReviewResult>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
	{
		return await dbContext.MaterialReviews
			.AsNoTracking()
			.Select(r => new MaterialReviewResult
			{
				MenteeId = r.ReviewerId,
				MaterialId = r.MaterialId,
				Rating = r.Rating,
			})
			.ToListAsync(cancellationToken);
	}
}

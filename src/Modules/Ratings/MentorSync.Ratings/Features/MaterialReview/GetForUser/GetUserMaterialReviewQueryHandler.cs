using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed class GetUserMaterialReviewQueryHandler(
	RatingsDbContext dbContext)
	: IQueryHandler<GetUserMaterialReviewQuery, UserMaterialReviewResponse>
{
	public async Task<Result<UserMaterialReviewResponse>> Handle(GetUserMaterialReviewQuery request, CancellationToken cancellationToken)
	{
		var review = await dbContext.MaterialReviews
				 .Where(mr => mr.MaterialId == request.MaterialId && mr.ReviewerId == request.UserId)
				 .Select(r => new UserMaterialReviewResponse(
					 r.Id,
					 r.Rating,
					 r.ReviewText,
					 r.CreatedAt,
					 r.UpdatedAt))
				 .FirstOrDefaultAsync(cancellationToken);

		if (review is null)
		{
			return Result.NotFound("Review not found for the specified material and user.");
		}

		return Result.Success(review);
	}
}

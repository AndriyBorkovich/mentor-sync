using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed class GetUserMaterialReviewQueryHandler(
    RatingsDbContext dbContext,
    ILogger<GetUserMaterialReviewQueryHandler> logger)
    : IRequestHandler<GetUserMaterialReviewQuery, Result<UserMaterialReviewResponse>>
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

        if (review == null)
        {
            return Result.NoContent();
        }

        return Result.Success(review);
    }
}

using Ardalis.Result;
using MediatR;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using MentorReviewEntityDto = MentorSync.Ratings.Features.MentorReview.Get.MentorReview;

namespace MentorSync.Ratings.Features.MentorReview.Get;

public class GetMentorReviewsQueryHandler(
    RatingsDbContext dbContext)
    : IRequestHandler<GetMentorReviewsQuery, Result<MentorReviewsResponse>>
{
    public async Task<Result<MentorReviewsResponse>> Handle(GetMentorReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await dbContext.Database
            .SqlQuery<MentorReviewEntityDto>($@"
                SELECT 
                mr.""Id"",
                mr.""Rating"",
                mr.""ReviewText"" AS ""Comment"",
                mr.""CreatedAt"" AS ""CreatedOn"",
                u.""UserName"" AS ""ReviewerName"",
                u.""ProfileImageUrl"" AS ""ReviewerImage""
                FROM ratings.""MentorReviews"" mr
                LEFT JOIN users.""Users"" u ON mr.""MenteeId"" = u.""Id""
                WHERE mr.""MentorId"" = {request.MentorId}")
            .OrderByDescending(mr => mr.CreatedOn)
            .Take(20)
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
}

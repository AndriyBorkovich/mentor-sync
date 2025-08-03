using Ardalis.Result;
using MentorSync.Ratings.Data;
using Microsoft.EntityFrameworkCore;
using MaterialReviewEntityDto = MentorSync.Ratings.Features.MaterialReview.Get.MaterialReview;

namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class GetMaterialReviewsQueryHandler(
	RatingsDbContext dbContext)
	: IQueryHandler<GetMaterialReviewsQuery, MaterialReviewsResponse>
{
	public async Task<Result<MaterialReviewsResponse>> Handle(GetMaterialReviewsQuery request, CancellationToken cancellationToken)
	{
		var reviews = await dbContext.Database.SqlQuery<MaterialReviewEntityDto>(
			$"""

            SELECT
            mr."Id",
            mr."Rating",
            mr."ReviewText" AS "Comment",
            mr."CreatedAt" AS "CreatedOn",
            u."UserName" AS "ReviewerName",
            u."ProfileImageUrl" AS "ReviewerImage",
            CASE
            WHEN EXISTS(
                SELECT 1
                FROM users."UserRoles" ur
                INNER JOIN users."Roles" r
                    ON ur."RoleId" = r."Id"
                WHERE ur."UserId" = u."Id"
                    AND r."Name" = 'Mentor'
            ) THEN TRUE
            ELSE FALSE
            END AS "IsReviewByMentor"
            FROM ratings."MaterialReviews" mr
            INNER JOIN users."Users" u
            ON mr."ReviewerId" = u."Id"
            WHERE mr."MaterialId" = {request.MaterialId}
            ORDER BY mr."CreatedAt" DESC
            LIMIT 20;
            """)
			.ToListAsync(cancellationToken);

		var stats = await dbContext.MaterialReviews
			.Where(mr => mr.MaterialId == request.MaterialId)
			.GroupBy(_ => true) // Group all reviews together
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
}

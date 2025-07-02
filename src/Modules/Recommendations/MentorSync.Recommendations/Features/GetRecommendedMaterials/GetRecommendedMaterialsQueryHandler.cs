using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

public sealed class GetRecommendedMaterialsQueryHandler(
    RecommendationsDbContext recommendationsContext)
        : IRequestHandler<GetRecommendedMaterialsQuery, Result<PaginatedList<RecommendedMaterialResponse>>>
{
    public async Task<Result<PaginatedList<RecommendedMaterialResponse>>> Handle(GetRecommendedMaterialsQuery request, CancellationToken cancellationToken)
    {
        var menteeId = request.MenteeId; var materialsQuery = recommendationsContext.Database
            .SqlQuery<RecommendedMaterialResultDto>($@"
                SELECT DISTINCT ON (lm.""Id"", lm.""Title"")
                    lm.""Id"",
                    lm.""Title"",
                    lm.""Description"",
                    lm.""Type""::text as ""Type"",
                    COALESCE(
                        (SELECT array_agg(t.""Name""::text)
                         FROM materials.""LearningMaterialTag"" lmt
                         JOIN materials.""Tags"" t ON lmt.""TagsId"" = t.""Id""
                         WHERE lmt.""LearningMaterialsId"" = lm.""Id""
                         GROUP BY lmt.""LearningMaterialsId""),
                        ARRAY[]::text[]
                    ) as ""Tags"",
                    lm.""MentorId"",
                    u.""UserName"" as ""MentorName"",
                    lm.""CreatedAt"",
                    mrr.""CollaborativeScore"",
                    mrr.""ContentBasedScore"",
                    mrr.""FinalScore""
                FROM materials.""LearningMaterials"" lm
                INNER JOIN users.""Users"" u ON lm.""MentorId"" = u.""Id""
                INNER JOIN recommendations.""MaterialRecommendationResults"" mrr ON lm.""Id"" = mrr.""MaterialId"" AND mrr.""MenteeId"" = {menteeId}
                WHERE mrr.""MenteeId"" = {menteeId} AND mrr.""FinalScore"" != 'NaN' AND mrr.""ContentBasedScore"" > 0.0 AND mrr.""CollaborativeScore"" > 0.0");

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            materialsQuery = materialsQuery.Where(m =>
                m.Title.ToLower().Contains(searchTerm) ||
                m.Description.ToLower().Contains(searchTerm) ||
                m.Tags.Any(t => t.ToLower().Contains(searchTerm)));
        }

        if (request.Tags != null && request.Tags.Count != 0)
        {
            materialsQuery = materialsQuery.Where(m =>
                m.Tags != null &&
                request.Tags.Any(tag =>
                    m.Tags.Contains(tag)));
        }

        if (request.Type.HasValue)
        {
            var materialType = request.Type.ToString();
            materialsQuery = materialsQuery.Where(m => m.Type == materialType);
        }

        materialsQuery = materialsQuery.OrderByDescending(m => m.FinalScore);

        var totalCount = await materialsQuery.CountAsync(cancellationToken);

        materialsQuery = materialsQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var materials = await materialsQuery.ToListAsync(cancellationToken);

        var items = materials.Select(m => new RecommendedMaterialResponse(
            m.Id,
            m.Title,
            m.Description,
            m.Type,
            [.. m.Tags],
            m.MentorId,
            m.MentorName,
            m.CreatedAt,
            float.IsNaN(m.CollaborativeScore) ? 0 : m.CollaborativeScore,
            float.IsNaN(m.ContentBasedScore) ? 0 : m.ContentBasedScore,
            float.IsNaN(m.FinalScore) ? 0 : m.FinalScore
        )).ToList();

        var paginatedList = new PaginatedList<RecommendedMaterialResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result.Success(paginatedList);
    }
}

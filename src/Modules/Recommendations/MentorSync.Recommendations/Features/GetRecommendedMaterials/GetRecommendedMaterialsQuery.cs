using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

/// <summary>
/// Query to get recommended learning materials for the currently logged-in mentee
/// </summary>
public sealed record GetRecommendedMaterialsQuery(
    int MenteeId,
    string SearchTerm = null,
    List<string> Tags = null,
    MaterialType? Type = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PaginatedList<RecommendedMaterialResponse>>>;

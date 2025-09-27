namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

/// <summary>
/// Query to get recommended learning materials for the currently logged-in mentee
/// </summary>
public sealed record GetRecommendedMaterialsQuery(
	int MenteeId,
	string SearchTerm = null,
	IReadOnlyList<string> Tags = null,
	MaterialType? Type = null,
	int PageNumber = 1,
	int PageSize = 10) : IQuery<PaginatedList<RecommendedMaterialResponse>>;

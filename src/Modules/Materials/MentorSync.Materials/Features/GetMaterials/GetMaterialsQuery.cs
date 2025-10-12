namespace MentorSync.Materials.Features.GetMaterials;

/// <summary>
/// Represents a query to retrieve a paginated list of materials, supporting optional filtering by search term, types, tags, and sorting.
/// </summary>
/// <param name="Search">The search term to filter materials by title or content.</param>
/// <param name="Types">A list of material types to filter by (e.g., "Article", "Video").</param>
/// <param name="Tags">A list of tags to filter materials.</param>
/// <param name="SortBy">The field to sort the results by (e.g., "CreatedAt", "Title").</param>
/// <param name="PageNumber">The page number for pagination (default is 1).</param>
/// <param name="PageSize">The number of items per page (default is 10).</param>
/// <example>
/// <code>
/// var query = new GetMaterialsQuery(
///     search: "C#",
///     types: new[] { "Article" },
///     tags: new[] { "Programming", "DotNet" },
///     sortBy: "CreatedAt",
///     pageNumber: 1,
///     pageSize: 20
/// );
/// </code>
/// </example>
public sealed record GetMaterialsQuery(
	string Search,
	IReadOnlyList<string> Types,
	IReadOnlyList<string> Tags,
	string SortBy,
	int PageNumber = 1,
	int PageSize = 10
) : IQuery<MaterialsResponse>;

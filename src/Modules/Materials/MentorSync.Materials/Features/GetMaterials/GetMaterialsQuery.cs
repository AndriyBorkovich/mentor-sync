namespace MentorSync.Materials.Features.GetMaterials;

public sealed record GetMaterialsQuery(
	string Search,
	List<string> Types,
	List<string> Tags,
	string SortBy,
	int PageNumber = 1,
	int PageSize = 10
) : IQuery<MaterialsResponse>;

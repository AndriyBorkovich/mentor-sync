using Ardalis.Result;
using MediatR;

namespace MentorSync.Materials.Features.GetMaterials;

public record GetMaterialsQuery(
    string Search,
    List<string> Types,
    List<string> Tags,
    string SortBy,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<MaterialsResponse>>;

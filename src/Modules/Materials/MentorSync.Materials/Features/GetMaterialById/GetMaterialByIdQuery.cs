namespace MentorSync.Materials.Features.GetMaterialById;

/// <summary>
/// Query to get a material by its ID
/// </summary>
/// <param name="Id">Material identifier</param>
public sealed record GetMaterialByIdQuery(int Id) : IQuery<MaterialResponse>;

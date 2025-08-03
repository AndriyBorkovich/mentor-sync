namespace MentorSync.Materials.Features.GetMaterialById;

public sealed record GetMaterialByIdQuery(int Id) : IQuery<MaterialResponse>;

namespace MentorSync.Materials.Features.GetMentorMaterials;

public sealed record GetMentorMaterialsQuery(int MentorId) : IQuery<MentorMaterialsResponse>;

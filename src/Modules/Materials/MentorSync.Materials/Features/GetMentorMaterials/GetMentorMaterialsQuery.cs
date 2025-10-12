namespace MentorSync.Materials.Features.GetMentorMaterials;

/// <summary>
/// Query to get all materials associated with a specific mentor
/// </summary>
/// <param name="MentorId">Mentor identifier</param>
public sealed record GetMentorMaterialsQuery(int MentorId) : IQuery<MentorMaterialsResponse>;

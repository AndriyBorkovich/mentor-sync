namespace MentorSync.Materials.Features.AddTags;

public sealed record AddTagsToMaterialCommand : ICommand<AddTagsResponse>
{
    public int MaterialId { get; init; }
    public List<string> TagNames { get; init; } = [];
    public int MentorId { get; init; } // To verify ownership
}

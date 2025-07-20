namespace MentorSync.Materials.Features.AddTags;

public sealed record AddTagsResponse
{
    public int MaterialId { get; init; }
    public List<TagInfo> Tags { get; init; } = [];
}

public sealed record TagInfo
{
    public int Id { get; init; }
    public string Name { get; init; }
}

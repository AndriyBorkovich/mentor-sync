namespace MentorSync.Materials.Features.AddTags;

public record AddTagsRequest
{
    public List<string> TagNames { get; init; } = new();
}

public record AddTagsResponse
{
    public int MaterialId { get; init; }
    public List<TagInfo> Tags { get; init; } = new();
}

public record TagInfo
{
    public int Id { get; init; }
    public string Name { get; init; }
}

namespace MentorSync.Materials.Features.GetMentorMaterials;

public record MentorMaterialsResponse
{
    public List<MaterialInfo> Materials { get; init; } = new();
}

public record MaterialInfo
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Type { get; init; }
    public string ContentMarkdown { get; init; }
    public string Url { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public List<MaterialAttachmentInfo> Attachments { get; init; } = new();
    public List<string> Tags { get; init; } = new();
}

public record MaterialAttachmentInfo
{
    public int Id { get; init; }
    public string FileName { get; init; }
    public string FileUrl { get; init; }
}

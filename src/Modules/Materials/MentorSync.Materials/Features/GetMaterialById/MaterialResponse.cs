namespace MentorSync.Materials.Features.GetMaterialById;

public record MaterialResponse
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Type { get; init; }
    public string ContentMarkdown { get; init; }
    public int MentorId { get; init; }
    public string MentorName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public List<AttachmentInfo> Attachments { get; init; } = new();
    public List<TagInfo> Tags { get; init; } = new();
}

public record AttachmentInfo
{
    public int Id { get; init; }
    public string FileName { get; init; }
    public string FileUrl { get; init; }
    public string ContentType { get; init; }
    public long FileSize { get; init; }
    public DateTime UploadedAt { get; init; }
}

public record TagInfo
{
    public int Id { get; init; }
    public string Name { get; init; }
}

namespace MentorSync.Materials.Features.GetMaterials;

public record MaterialsResponse
{
    public List<MaterialDto> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public record MaterialDto
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
    public List<AttachmentDto> Attachments { get; init; } = [];
    public List<TagDto> Tags { get; init; } = [];
}

public record AttachmentDto
{
    public int Id { get; init; }
    public string FileName { get; init; }
    public string FileUrl { get; init; }
    public string ContentType { get; init; }
    public long FileSize { get; init; }
    public DateTime UploadedAt { get; init; }
}

public record TagDto
{
    public int Id { get; init; }
    public string Name { get; init; }
}

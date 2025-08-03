namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public sealed record UploadAttachmentResponse
{
	public int Id { get; init; }
	public string FileName { get; init; }
	public string FileUrl { get; init; }
	public string ContentType { get; init; }
	public long FileSize { get; init; }
	public DateTime UploadedAt { get; init; }
}

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

/// <summary>
/// Response returned after uploading an attachment to a material
/// </summary>
public sealed record UploadAttachmentResponse
{
	/// <summary>
	/// Unique identifier of the uploaded attachment
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// Name of the uploaded file
	/// </summary>
	public string FileName { get; init; }
	/// <summary>
	/// URL where the uploaded file can be accessed
	/// </summary>
	public string FileUrl { get; init; }
	/// <summary>
	/// MIME content type of the uploaded file
	/// </summary>
	public string ContentType { get; init; }
	/// <summary>
	/// Size of the uploaded file in bytes
	/// </summary>
	public long FileSize { get; init; }
	/// <summary>
	/// Timestamp when the file was uploaded
	/// </summary>
	public DateTime UploadedAt { get; init; }
}

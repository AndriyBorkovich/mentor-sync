namespace MentorSync.Materials.Domain;

/// <summary>
/// Attachments for materials (e.g., images, PDFs) stored in Blob storage.
/// </summary>
public sealed class MaterialAttachment
{
	/// <summary>
	/// Unique identifier for the attachment
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Reference to the learning material this attachment belongs to
	/// </summary>
	public int MaterialId { get; set; }
	/// <summary>
	/// Original file name of the attachment
	/// </summary>
	public string FileName { get; set; }
	/// <summary>
	/// Blob URI for the attachment in Azure Blob Storage
	/// </summary>
	public string BlobUri { get; set; }
	/// <summary>
	/// MIME type of the attachment (e.g., "image/png", "application/pdf")
	/// </summary>
	public string ContentType { get; set; }
	/// <summary>
	/// Size of the attachment in bytes
	/// </summary>
	public long Size { get; set; }
	/// <summary>
	/// Timestamp when the attachment was uploaded
	/// </summary>
	public DateTime UploadedAt { get; set; }

	/// <summary>
	/// Navigation property to the associated learning material
	/// </summary>
	public LearningMaterial Material { get; set; }
}

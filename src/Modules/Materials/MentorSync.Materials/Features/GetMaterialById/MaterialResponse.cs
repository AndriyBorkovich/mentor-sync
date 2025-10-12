namespace MentorSync.Materials.Features.GetMaterialById;

/// <summary>
/// Represents the response containing detailed information about a material, including its attachments and tags.
/// </summary>
public sealed record MaterialResponse
{
	/// <summary>
	/// Gets the unique identifier of the material.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets the title of the material.
	/// </summary>
	public string Title { get; init; }

	/// <summary>
	/// Gets the description of the material.
	/// </summary>
	public string Description { get; init; }

	/// <summary>
	/// Gets the type of the material (e.g., article, video).
	/// </summary>
	public string Type { get; init; }

	/// <summary>
	/// Gets the markdown content of the material.
	/// </summary>
	public string ContentMarkdown { get; init; }

	/// <summary>
	/// Gets the unique identifier of the mentor who created the material.
	/// </summary>
	public int MentorId { get; init; }

	/// <summary>
	/// Gets the name of the mentor who created the material.
	/// </summary>
	public string MentorName { get; init; }

	/// <summary>
	/// Gets the date and time when the material was created.
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Gets the date and time when the material was last updated, if available.
	/// </summary>
	public DateTime? UpdatedAt { get; init; }

	/// <summary>
	/// Gets the list of attachments associated with the material.
	/// </summary>
	public IReadOnlyList<AttachmentInfo> Attachments { get; init; } = [];

	/// <summary>
	/// Gets the list of tags associated with the material.
	/// </summary>
	public IReadOnlyList<TagInfo> Tags { get; init; } = [];
}

/// <summary>
/// Represents information about an attachment related to a material.
/// </summary>
public sealed record AttachmentInfo
{
	/// <summary>
	/// Gets the unique identifier of the attachment.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets the file name of the attachment.
	/// </summary>
	public string FileName { get; init; }

	/// <summary>
	/// Gets the URL where the attachment can be accessed.
	/// </summary>
	public string FileUrl { get; init; }

	/// <summary>
	/// Gets the MIME content type of the attachment.
	/// </summary>
	public string ContentType { get; init; }

	/// <summary>
	/// Gets the size of the attachment file in bytes.
	/// </summary>
	public long FileSize { get; init; }

	/// <summary>
	/// Gets the date and time when the attachment was uploaded.
	/// </summary>
	public DateTime UploadedAt { get; init; }
}

/// <summary>
/// Represents a tag associated with a material.
/// </summary>
public sealed record TagInfo
{
	/// <summary>
	/// Gets the unique identifier of the tag.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets the name of the tag.
	/// </summary>
	public string Name { get; init; }
}

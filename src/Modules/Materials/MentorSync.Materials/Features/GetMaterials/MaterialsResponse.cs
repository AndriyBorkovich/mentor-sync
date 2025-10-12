namespace MentorSync.Materials.Features.GetMaterials;

/// <summary>
/// Represents a paginated response containing a list of materials.
/// </summary>
/// <remarks>
/// Includes pagination metadata and the collection of <see cref="MaterialDto"/> items.
/// </remarks>
/// <example>
/// <code>
/// var response = new MaterialsResponse
/// {
///     Items = new List&lt;MaterialDto&gt; { /* ... */ },
///     TotalCount = 100,
///     PageSize = 10,
///     PageNumber = 1
/// };
/// </code>
/// </example>
public sealed record MaterialsResponse
{
	/// <summary>
	/// Gets the list of materials for the current page.
	/// </summary>
	public IReadOnlyList<MaterialDto> Items { get; init; } = [];

	/// <summary>
	/// Gets the total number of materials available.
	/// </summary>
	public int TotalCount { get; init; }

	/// <summary>
	/// Gets the number of items per page.
	/// </summary>
	public int PageSize { get; init; }

	/// <summary>
	/// Gets the current page number.
	/// </summary>
	public int PageNumber { get; init; }

	/// <summary>
	/// Gets the total number of pages based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
	/// </summary>
	public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

/// <summary>
/// Represents a material item with its details, attachments, and tags.
/// </summary>
/// <example>
/// <code>
/// var material = new MaterialDto
/// {
///     Id = 1,
///     Title = "Introduction to C#",
///     Description = "A beginner's guide.",
///     Type = "Article",
///     ContentMarkdown = "# Welcome",
///     MentorId = 42,
///     MentorName = "Jane Doe",
///     CreatedAt = DateTime.UtcNow,
///     Attachments = new List&lt;AttachmentDto&gt;(),
///     Tags = new List&lt;TagDto&gt;()
/// };
/// </code>
/// </example>
public sealed record MaterialDto
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
	/// Gets the type of the material (e.g., Article, Video).
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
	public IReadOnlyList<AttachmentDto> Attachments { get; init; } = [];

	/// <summary>
	/// Gets the list of tags associated with the material.
	/// </summary>
	public IReadOnlyList<TagDto> Tags { get; init; } = [];
}

/// <summary>
/// Represents an attachment associated with a material.
/// </summary>
/// <example>
/// <code>
/// var attachment = new AttachmentDto
/// {
///     Id = 10,
///     FileName = "slides.pdf",
///     FileUrl = "https://example.com/slides.pdf",
///     ContentType = "application/pdf",
///     FileSize = 204800,
///     UploadedAt = DateTime.UtcNow
/// };
/// </code>
/// </example>
public sealed record AttachmentDto
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
/// <example>
/// <code>
/// var tag = new TagDto
/// {
///     Id = 5,
///     Name = "CSharp"
/// };
/// </code>
/// </example>
public sealed record TagDto
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

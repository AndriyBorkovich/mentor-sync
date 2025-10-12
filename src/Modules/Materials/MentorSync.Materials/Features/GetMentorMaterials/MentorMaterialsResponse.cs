namespace MentorSync.Materials.Features.GetMentorMaterials;

/// <summary>
/// Represents the response containing a list of materials authored by a mentor.
/// </summary>
/// <example>
/// <code>
/// var response = new MentorMaterialsResponse
/// {
///     Materials = new List&lt;MaterialInfo&gt; { /* ... */ }
/// };
/// </code>
/// </example>
public sealed record MentorMaterialsResponse
{
	/// <summary>
	/// Gets the list of materials created by the mentor.
	/// </summary>
	public IList<MaterialInfo> Materials { get; init; } = [];
}

/// <summary>
/// Represents detailed information about a material authored by a mentor.
/// </summary>
/// <example>
/// <code>
/// var material = new MaterialInfo
/// {
///     Id = 1,
///     Title = "Async Programming in C#",
///     Description = "An overview of async/await.",
///     Type = "Article",
///     ContentMarkdown = "# Async in C#",
///     Url = "https://example.com/material/1",
///     CreatedOn = DateTime.UtcNow,
///     Attachments = new List&lt;MaterialAttachmentInfo&gt;(),
///     Tags = new List&lt;string&gt; { "CSharp", "Async" }
/// };
/// </code>
/// </example>
public sealed record MaterialInfo
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
	/// Gets the URL to access the material.
	/// </summary>
	public string Url { get; init; }

	/// <summary>
	/// Gets the date and time when the material was created.
	/// </summary>
	public DateTime CreatedOn { get; init; }

	/// <summary>
	/// Gets the date and time when the material was last updated, if available.
	/// </summary>
	public DateTime? UpdatedOn { get; init; }

	/// <summary>
	/// Gets the list of attachments associated with the material.
	/// </summary>
	public List<MaterialAttachmentInfo> Attachments { get; init; } = [];

	/// <summary>
	/// Gets the list of tags associated with the material.
	/// </summary>
	public List<string> Tags { get; init; } = [];
}

/// <summary>
/// Represents information about an attachment related to a material.
/// </summary>
/// <example>
/// <code>
/// var attachment = new MaterialAttachmentInfo
/// {
///     Id = 10,
///     FileName = "example.pdf",
///     FileUrl = "https://example.com/files/example.pdf"
/// };
/// </code>
/// </example>
public sealed record MaterialAttachmentInfo
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
}

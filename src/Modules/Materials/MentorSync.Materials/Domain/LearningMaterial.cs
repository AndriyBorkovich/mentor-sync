namespace MentorSync.Materials.Domain;

/// <summary>
/// Represents a learning material entity in the system
/// </summary>
public sealed class LearningMaterial
{
	/// <summary>
	/// Gets or sets the unique identifier of the learning material
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Gets or sets the title of the learning material
	/// </summary>
	public string Title { get; init; }

	/// <summary>
	/// Gets or sets the description of the learning material
	/// </summary>
	public string Description { get; init; }

	/// <summary>
	/// Gets or sets the type of learning material (Article, Video, Document)
	/// </summary>
	public MaterialType Type { get; init; }

	/// <summary>
	/// Text in markdown (.md)
	/// </summary>
	public string ContentMarkdown { get; init; }

	/// <summary>
	/// Reference to mentor, who created this material
	/// </summary>
	public int MentorId { get; init; }

	/// <summary>
	/// Gets or sets when the learning material was created
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Gets or sets when the learning material was last updated
	/// </summary>
	public DateTime? UpdatedAt { get; init; }

	/// <summary>
	/// Gets or sets the collection of file attachments associated with this material
	/// </summary>
	public ICollection<MaterialAttachment> Attachments { get; init; }

	/// <summary>
	/// Gets or sets the collection of tags associated with this material
	/// </summary>
	public ICollection<Tag> Tags { get; set; }
}

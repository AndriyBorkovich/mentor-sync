namespace MentorSync.Materials.Domain;

/// <summary>
/// Represents a learning material entity in the system
/// </summary>
public sealed class LearningMaterial
{
	/// <summary>
	/// Gets or sets the unique identifier of the learning material
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the title of the learning material
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// Gets or sets the description of the learning material
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Gets or sets the type of learning material (Article, Video, Document)
	/// </summary>
	public MaterialType Type { get; set; }

	/// <summary>
	/// Text in markdown format (.md)
	/// </summary>
	public string ContentMarkdown { get; set; }

	/// <summary>
	/// Reference to mentor, who created this material
	/// </summary>
	public int MentorId { get; set; }

	/// <summary>
	/// Gets or sets when the learning material was created
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets when the learning material was last updated
	/// </summary>
	public DateTime? UpdatedAt { get; set; }

	/// <summary>
	/// Gets or sets the collection of file attachments associated with this material
	/// </summary>
	public List<MaterialAttachment> Attachments { get; set; }

	/// <summary>
	/// Gets or sets the collection of tags associated with this material
	/// </summary>
	public List<Tag> Tags { get; set; }
}

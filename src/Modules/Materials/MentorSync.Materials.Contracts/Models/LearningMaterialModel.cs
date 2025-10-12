namespace MentorSync.Materials.Contracts.Models;

/// <summary>
/// Model representing a learning material for recommendation purposes.
/// </summary>
public sealed class LearningMaterialModel
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
	/// Gets or sets the type of the learning material (Article, Video, Document)
	/// </summary>
	public MaterialType Type { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the mentor who created this material
	/// </summary>
	public int MentorId { get; set; }

	/// <summary>
	/// Gets or sets the creation timestamp of the learning material
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the list of tags associated with this learning material
	/// </summary>
	public ICollection<string> Tags { get; set; } = [];
}

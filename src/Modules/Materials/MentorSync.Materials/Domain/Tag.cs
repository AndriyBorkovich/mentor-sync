namespace MentorSync.Materials.Domain;

/// <summary>
/// Represents a tag entity for categorizing learning materials
/// </summary>
public sealed class Tag
{
	/// <summary>
	/// Gets or sets the unique identifier of the tag
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the tag
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the description of the tag
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Gets or sets the collection of learning materials associated with this tag
	/// </summary>
	public ICollection<LearningMaterial> LearningMaterials { get; set; }
}

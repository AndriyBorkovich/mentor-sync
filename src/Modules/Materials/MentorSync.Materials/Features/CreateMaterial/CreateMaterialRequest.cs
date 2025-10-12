namespace MentorSync.Materials.Features.CreateMaterial;

/// <summary>
/// Request to create a new material
/// </summary>
public sealed record CreateMaterialRequest
{
	/// <summary>
	/// Title of the learning material
	/// </summary>
	public string Title { get; init; }
	/// <summary>
	/// Description of the learning material
	/// </summary>
	public string Description { get; init; }
	/// <summary>
	/// Type of learning material (Article, Video, Document)
	/// </summary>
	public string Type { get; init; }
	/// <summary>
	/// Text in markdown (.md)
	/// </summary>
	public string ContentMarkdown { get; init; }
	/// <summary>
	/// Reference to mentor, who created this material
	/// </summary>
	public int MentorId { get; init; }
	/// <summary>
	/// List of tags associated with this material
	/// </summary>
	public IReadOnlyList<string> Tags { get; } = [];
}

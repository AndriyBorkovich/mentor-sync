using System.ComponentModel.DataAnnotations;

namespace MentorSync.Materials.Features.CreateMaterial;

/// <summary>
/// Command for creating a new learning material
/// </summary>
public sealed record CreateMaterialCommand : ICommand<CreateMaterialResponse>
{
	/// <summary>
	/// Gets the title of the learning material
	/// </summary>
	[Required]
	[StringLength(200)]
	public string Title { get; init; }

	/// <summary>
	/// Gets the description of the learning material
	/// </summary>
	[StringLength(2000)]
	public string Description { get; init; }

	/// <summary>
	/// Gets the type of learning material (Article, Video, Document)
	/// </summary>
	[Required]
	public MaterialType Type { get; init; }

	/// <summary>
	/// Gets the content in markdown format
	/// </summary>
	public string ContentMarkdown { get; init; }

	/// <summary>
	/// Gets the identifier of the mentor creating this material
	/// </summary>
	[Required]
	public int MentorId { get; init; }

	/// <summary>
	/// Gets the list of tags to associate with this material
	/// </summary>
	public IReadOnlyList<string> Tags { get; init; } = [];
}

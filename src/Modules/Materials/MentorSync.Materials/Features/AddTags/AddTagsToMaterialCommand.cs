namespace MentorSync.Materials.Features.AddTags;

/// <summary>
/// Command to add tags to a material
/// </summary>
public sealed record AddTagsToMaterialCommand : ICommand<AddTagsResponse>
{
	/// <summary>
	/// The ID of the material to which tags will be added
	/// </summary>
	public int MaterialId { get; init; }
	/// <summary>
	/// The names of the tags to be added to the material
	/// </summary>
	public IReadOnlyList<string> TagNames { get; init; } = [];
	/// <summary>
	/// The ID of the mentor adding the tags
	/// </summary>
	/// <remarks>To verify ownership</remarks>
	public int MentorId { get; init; }
}

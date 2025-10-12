namespace MentorSync.Materials.Features.AddTags;

/// <summary>
/// Request to add tags to materials
/// </summary>
public sealed record AddTagsRequest
{
	/// <summary>
	/// The IDs of the materials to which the tags will be added
	/// </summary>
	public IReadOnlyList<string> TagNames { get; init; } = [];
}

namespace MentorSync.Materials.Features.AddTags;

/// <summary>
/// Response returned after adding tags to a material
/// </summary>
public sealed record AddTagsResponse
{
	/// <summary>
	/// The ID of the material to which tags were added
	/// </summary>
	public int MaterialId { get; init; }
	/// <summary>
	/// The list of tags that were added to the material
	/// </summary>
	public IReadOnlyList<TagInfo> Tags { get; init; } = [];
}

/// <summary>
/// Information about a tag
/// </summary>
public sealed record TagInfo
{
	/// <summary>
	/// The ID of the tag
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// The name of the tag
	/// </summary>
	public string Name { get; init; }
}

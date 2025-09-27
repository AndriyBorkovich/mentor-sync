namespace MentorSync.Materials.Features.AddTags;

public sealed record AddTagsRequest
{
	public IReadOnlyList<string> TagNames { get; init; } = [];
}

namespace MentorSync.Materials.Features.AddTags;

public sealed record AddTagsRequest
{
	public List<string> TagNames { get; init; } = [];
}

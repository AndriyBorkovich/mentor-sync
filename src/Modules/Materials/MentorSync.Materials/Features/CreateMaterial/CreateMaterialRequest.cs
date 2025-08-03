namespace MentorSync.Materials.Features.CreateMaterial;

public sealed record CreateMaterialRequest
{
	public string Title { get; init; }
	public string Description { get; init; }
	public string Type { get; init; }
	public string ContentMarkdown { get; init; }
	public int MentorId { get; init; }
	public IReadOnlyList<string> Tags { get; init; } = [];
}

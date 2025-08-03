namespace MentorSync.Materials.Features.CreateMaterial;

public sealed record CreateMaterialResponse
{
	public int Id { get; init; }
	public string Title { get; init; }
	public DateTime CreatedAt { get; init; }
}

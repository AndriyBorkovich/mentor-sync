namespace MentorSync.Materials.Features.CreateMaterial;

/// <summary>
/// Response returned after creating a new material
/// </summary>
public sealed record CreateMaterialResponse
{
	/// <summary>
	/// Unique identifier of the newly created material
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// Title of the newly created material
	/// </summary>
	public string Title { get; init; }
	/// <summary>
	/// Description of the newly created material
	/// </summary>
	public DateTime CreatedAt { get; init; }
}

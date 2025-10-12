namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks when a mentee views a learning material (for recommendation algorithms).
/// </summary>
public sealed class MaterialViewEvent : BaseViewEvent
{
	/// <summary>
	/// Identifier of the material being viewed.
	/// </summary>
	public int MaterialId { get; set; }
}

namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks when a mentee likes a learning material (for recommendation algorithms).
/// </summary>
public sealed class MaterialLike
{
	/// <summary>
	/// Unique identifier for the material like event.
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Identifier of the mentee who liked the material.
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Identifier of the liked learning material.
	/// </summary>
	public int MaterialId { get; set; }
	/// <summary>
	/// Timestamp of when the material was liked.
	/// </summary>
	public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}

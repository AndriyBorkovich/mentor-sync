namespace MentorSync.Recommendations.Domain.Interaction;

/// <summary>
/// Base class for interactions between mentees and other entities (mentors, materials, etc.)
/// </summary>
public class BaseInteraction
{
	/// <summary>
	/// Unique identifier for the interaction
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Identifier for the mentee involved in the interaction
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Aggregated score from events (views, bookmarks, likes, etc.) between a mentee and other entities (mentors, materials, etc.).
	/// </summary>
	public float Score { get; set; }
	/// <summary>
	/// Timestamp of when the interaction was created
	/// </summary>
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

namespace MentorSync.Recommendations.Domain.Interaction;

public class BaseInteraction
{
	public int Id { get; set; }
	public int MenteeId { get; set; }
	/// <summary>
	/// Aggregated score from events (views, bookmarks, likes, etc.) between a mentee and other entities (mentors, materials, etc.).
	/// </summary>
	public float Score { get; set; }
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

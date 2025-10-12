namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Base class for view events
/// </summary>
public class BaseViewEvent
{
	/// <summary>
	/// Unique identifier for the view event
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Identifier of the mentee who viewed the mentor
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Timestamp when the view event occurred
	/// </summary>
	public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}

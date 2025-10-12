namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

/// <summary>
/// Response model for mentor's upcoming sessions
/// </summary>
public sealed record MentorUpcomingSessionsResponse
{
	/// <summary>
	/// List of upcoming sessions for the mentor
	/// </summary>
	public IReadOnlyList<SessionInfo> UpcomingSessions { get; init; } = [];
}

/// <summary>
/// Information about a single session
/// </summary>
public sealed record SessionInfo
{
	/// <summary>
	/// Unique identifier for the session
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// Title of the session
	/// </summary>
	public string Title { get; init; }
	/// <summary>
	/// Description of the session
	/// </summary>
	public string Description { get; init; }
	/// <summary>
	/// Start time of the session
	/// </summary>
	public DateTimeOffset StartTime { get; init; }
	/// <summary>
	/// End time of the session
	/// </summary>
	public DateTimeOffset EndTime { get; init; }
	/// <summary>
	/// Status of the session (e.g., Confirmed, Pending, Cancelled)
	/// </summary>
	public string Status { get; init; }
	/// <summary>
	/// Name of the mentee
	/// </summary>
	public string MenteeName { get; init; }
	/// <summary>
	/// Image URL of the mentee
	/// </summary>
	public string MenteeImage { get; init; }
}

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public sealed record MentorUpcomingSessionsResponse
{
	public IReadOnlyList<SessionInfo> UpcomingSessions { get; init; } = [];
}

public sealed record SessionInfo
{
	public int Id { get; init; }
	public string Title { get; init; }
	public string Description { get; init; }
	public DateTimeOffset StartTime { get; init; }
	public DateTimeOffset EndTime { get; init; }
	public string Status { get; init; }
	public string MenteeName { get; init; }
	public string MenteeImage { get; init; }
}

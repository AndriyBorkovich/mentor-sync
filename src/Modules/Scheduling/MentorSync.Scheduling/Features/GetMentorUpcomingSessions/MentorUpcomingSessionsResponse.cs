using System;
using System.Collections.Generic;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public record MentorUpcomingSessionsResponse
{
    public List<SessionInfo> UpcomingSessions { get; init; } = [];
}

public record SessionInfo
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

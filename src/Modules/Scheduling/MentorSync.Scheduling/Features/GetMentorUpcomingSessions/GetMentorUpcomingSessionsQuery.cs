namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

/// <summary>
/// Query to get upcoming sessions for a mentor
/// </summary>
/// <param name="MentorId">Mentor identifier</param>
public sealed record GetMentorUpcomingSessionsQuery(int MentorId) : IQuery<MentorUpcomingSessionsResponse>;

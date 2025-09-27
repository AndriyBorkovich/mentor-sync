namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public sealed record GetMentorUpcomingSessionsQuery(int MentorId) : IQuery<MentorUpcomingSessionsResponse>;

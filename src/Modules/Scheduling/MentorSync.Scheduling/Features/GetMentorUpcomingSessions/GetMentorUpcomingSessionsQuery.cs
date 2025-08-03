namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public record GetMentorUpcomingSessionsQuery(int MentorId) : IQuery<MentorUpcomingSessionsResponse>;

using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public record GetMentorUpcomingSessionsQuery(int MentorId) : IRequest<Result<MentorUpcomingSessionsResponse>>;

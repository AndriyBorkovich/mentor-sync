using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public sealed class GetMentorUpcomingSessionsQueryHandler(
    SchedulingDbContext dbContext)
    : IRequestHandler<GetMentorUpcomingSessionsQuery, Result<MentorUpcomingSessionsResponse>>
{
    public async Task<Result<MentorUpcomingSessionsResponse>> Handle(GetMentorUpcomingSessionsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var sessions = await dbContext.Bookings
            .Where(b =>
                b.MentorId == request.MentorId &&
                b.Start > now &&
                b.Status == BookingStatus.Pending)
            .OrderBy(b => b.Start)
            .Select(booking => new SessionInfo
            {
                Id = booking.Id,
                Title = $"Session with",
                Description = "Mentoring Session",
                StartTime = booking.Start,
                EndTime = booking.End,
                Status = booking.Status.ToString(),
            })
            .Take(5)
            .ToListAsync(cancellationToken);

        // Create response
        var response = new MentorUpcomingSessionsResponse
        {
            UpcomingSessions = sessions
        };

        return Result.Success(response);
    }
}

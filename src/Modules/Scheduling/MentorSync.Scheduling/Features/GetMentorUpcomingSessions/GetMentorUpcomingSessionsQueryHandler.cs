using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel.CommonEntities.Enums;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public sealed class GetMentorUpcomingSessionsQueryHandler(
    SchedulingDbContext dbContext)
    : IRequestHandler<GetMentorUpcomingSessionsQuery, Result<MentorUpcomingSessionsResponse>>
{
    public async Task<Result<MentorUpcomingSessionsResponse>> Handle(GetMentorUpcomingSessionsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var sessions = await dbContext.Database
            .SqlQuery<SessionInfo>(
            $@"
                SELECT 
                b.""Id"",
                'Сесія з ' || u.""UserName"" AS ""Title"",
                'Mentoring Session' AS ""Description"",
                b.""Start"" AS ""StartTime"",
                b.""End"" AS ""EndTime"",
                CAST(b.""Status"" AS varchar) AS ""Status"",
                u.""UserName"" AS ""MenteeName"",
                u.""ProfileImageUrl"" AS ""MenteeImage""
                FROM scheduling.""Bookings"" b
                INNER JOIN users.""Users"" u ON b.""MenteeId"" = u.""Id""
                WHERE 
                b.""MentorId"" = {request.MentorId}
                AND b.""Status"" = {BookingStatus.Pending.ToString()}
                ORDER BY b.""Start""
                OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY")
            .Where(s => s.StartTime > now)
            .ToListAsync(cancellationToken);


        return Result.Success(new MentorUpcomingSessionsResponse
        {
            UpcomingSessions = sessions
        });
    }
}

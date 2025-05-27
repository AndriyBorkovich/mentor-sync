using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public class GetMentorUpcomingSessionsQueryHandler : IRequestHandler<GetMentorUpcomingSessionsQuery, Result<MentorUpcomingSessionsResponse>>
{
    private readonly SchedulingDbContext _dbContext;
    private readonly ILogger<GetMentorUpcomingSessionsQueryHandler> _logger;

    public GetMentorUpcomingSessionsQueryHandler(SchedulingDbContext dbContext, ILogger<GetMentorUpcomingSessionsQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MentorUpcomingSessionsResponse>> Handle(GetMentorUpcomingSessionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var now = DateTimeOffset.UtcNow;

            var sessions = await _dbContext.Bookings
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
                    Status = booking.Status.ToString()
                }
                )
                .Take(5)
                .ToListAsync(cancellationToken);

            // Create response
            var response = new MentorUpcomingSessionsResponse
            {
                UpcomingSessions = sessions
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming sessions for MentorId: {MentorId}", request.MentorId);
            return Result.Error($"An error occurred while getting upcoming sessions: {ex.Message}");
        }
    }
}

using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Features.GetUserBookings;

public sealed class GetMenteeBookingsQueryHandler(
    SchedulingDbContext dbContext,
    ILogger<GetMenteeBookingsQueryHandler> logger)
    : IRequestHandler<GetMenteeBookingsQuery, Result<UserBookingsResponse>>
{
    public async Task<Result<UserBookingsResponse>> Handle(GetMenteeBookingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all bookings for the mentee
            var bookings = await dbContext.Bookings
                .Where(b => b.MenteeId == request.MenteeId)
                .OrderByDescending(b => b.Start) // Most recent first
                .ToListAsync(cancellationToken);

            // Create response
            var response = new UserBookingsResponse
            {
                Bookings = bookings.Select(booking => new BookingDto
                {
                    Id = booking.Id,
                    MentorId = booking.MentorId,
                    MentorName = $"Mentor {booking.MentorId}", // Should be populated from the Users module
                    MenteeId = booking.MenteeId,
                    MenteeName = $"Mentee {booking.MenteeId}", // Should be populated from the Users module
                    Start = booking.Start,
                    End = booking.End,
                    Status = booking.Status,
                    CreatedAt = booking.CreatedAt,
                    UpdatedAt = booking.UpdatedAt
                }).ToList()
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving mentee bookings for MenteeId: {MenteeId}", request.MenteeId);
            return Result.Error($"An error occurred while retrieving mentee bookings: {ex.Message}");
        }
    }
}

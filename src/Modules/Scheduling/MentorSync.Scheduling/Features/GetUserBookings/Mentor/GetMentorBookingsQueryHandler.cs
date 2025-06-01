using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

public sealed class GetMentorBookingsQueryHandler(
    SchedulingDbContext dbContext,
    ILogger<GetMentorBookingsQueryHandler> logger)
    : IRequestHandler<GetMentorBookingsQuery, Result<UserBookingsResponse>>
{
    public async Task<Result<UserBookingsResponse>> Handle(GetMentorBookingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all bookings for the mentor
            var bookings = await dbContext.Bookings
                .Where(b => b.MentorId == request.MentorId)
                .OrderByDescending(b => b.Start) // Most recent first
                .ToListAsync(cancellationToken);

            // Create response
            var response = new UserBookingsResponse
            {
                Bookings = [.. bookings.Select(booking => new BookingDto
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
                })]
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving mentor bookings for MentorId: {MentorId}", request.MentorId);
            return Result.Error($"An error occurred while retrieving mentor bookings: {ex.Message}");
        }
    }
}

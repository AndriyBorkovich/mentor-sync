using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Services;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookingStatus = MentorSync.SharedKernel.CommonEntities.BookingStatus;

namespace MentorSync.Scheduling.Features.Booking;

public sealed class ConfirmBookingCommandHandler(
    SchedulingDbContext dbContext,
    ILogger<ConfirmBookingCommandHandler> logger,
    INotificationService notificationService)
    : IRequestHandler<ConfirmBookingCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return Result.NotFound($"Booking with ID {request.BookingId} not found");
            }

            if (booking.Status != BookingStatus.Pending)
            {
                return Result.Error($"Booking status is {booking.Status}, only pending bookings can be confirmed");
            }

            booking.Status = BookingStatus.Confirmed;
            booking.UpdatedAt = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            // Notify both mentor and mentee
            await notificationService.SendBookingStatusChangedNotificationAsync(
                booking.Id,
                booking.Status.ToString(),
                "Booking Confirmed",
                booking.Start.UtcDateTime,
                booking.End.UtcDateTime,
                booking.MentorId,
                "Your booking has been confirmed."
            );
            await notificationService.SendBookingStatusChangedNotificationAsync(
                booking.Id,
                booking.Status.ToString(),
                "Booking Confirmed",
                booking.Start.UtcDateTime,
                booking.End.UtcDateTime,
                booking.MenteeId,
                "Your booking has been confirmed."
            );

            return Result.Success($"Booking {request.BookingId} confirmed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error confirming booking {BookingId}", request.BookingId);
            return Result.Error($"An error occurred while confirming the booking: {ex.Message}");
        }
    }
}

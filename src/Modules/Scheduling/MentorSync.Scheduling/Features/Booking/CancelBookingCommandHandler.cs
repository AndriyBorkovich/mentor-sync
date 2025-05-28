using Ardalis.Result;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Features.Booking;

public sealed class CancelBookingCommandHandler(
    SchedulingDbContext dbContext,
    ILogger<CancelBookingCommandHandler> logger,
    IMediator mediator)
    : IRequestHandler<CancelBookingCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return Result.NotFound($"Booking with ID {request.BookingId} not found");
            }

            // Can only cancel pending or confirmed bookings
            if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            {
                return Result.Error($"Booking status is {booking.Status}, only pending or confirmed bookings can be cancelled");
            }

            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTimeOffset.UtcNow;

            // When a booking is cancelled, make the slot available again
            var availabilitySlot = await dbContext.MentorAvailabilities
                .FirstOrDefaultAsync(a =>
                    a.MentorId == booking.MentorId &&
                    a.Start == booking.Start &&
                    a.End == booking.End,
                cancellationToken);

            if (availabilitySlot == null)
            {
                // Create a new availability slot with the same time range
                dbContext.MentorAvailabilities.Add(new Domain.MentorAvailability
                {
                    MentorId = booking.MentorId,
                    Start = booking.Start,
                    End = booking.End,
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // Send notification to other user (if mentor cancels, notify student and vice versa)
            var recipientId = request.UserId == booking.MentorId ? booking.MenteeId : booking.MentorId;

            // Send notification via SignalR
            await mediator.Send(new SendBookingStatusChangedNotificationCommand
            {
                UserId = recipientId.ToString(),
                Notification = new BookingStatusChangedNotification(
                    BookingId: booking.Id,
                    NewStatus: booking.Status.ToString(),
                    Title: "Booking Cancelled",
                    StartTime: booking.Start.DateTime,
                    EndTime: booking.End.DateTime,
                    MentorName: null, // These could be populated if you fetch user details
                    StudentName: null,
                    Message: "A booking has been cancelled"
                )
            }, cancellationToken);

            return Result.Success($"Booking {request.BookingId} cancelled successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling booking {BookingId}", request.BookingId);
            return Result.Error($"An error occurred while cancelling the booking: {ex.Message}");
        }
    }
}

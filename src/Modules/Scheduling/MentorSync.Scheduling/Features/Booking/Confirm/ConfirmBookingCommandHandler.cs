using Ardalis.Result;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Services;
using Microsoft.EntityFrameworkCore;
using BookingStatus = MentorSync.SharedKernel.CommonEntities.Enums.BookingStatus;

namespace MentorSync.Scheduling.Features.Booking.Confirm;

/// <summary>
/// Handles the confirmation of a booking request
/// </summary>
/// <param name="dbContext">Database context</param>
/// <param name="notificationService">Notification service</param>
public sealed class ConfirmBookingCommandHandler(
	SchedulingDbContext dbContext,
	INotificationService notificationService)
	: ICommandHandler<ConfirmBookingCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken = default)
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
}

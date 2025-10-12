using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.Booking.UpdatePending;

/// <summary>
/// Handler for updating pending bookings to no-show status if the start time has passed.
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class UpdatePendingBookingsCommandHandler(SchedulingDbContext dbContext)
: ICommandHandler<UpdatePendingBookingsCommand, int>
{
	/// <inheritdoc />
	public async Task<Result<int>> Handle(UpdatePendingBookingsCommand request, CancellationToken cancellationToken = default)
	{
		var now = DateTimeOffset.UtcNow;

		// update all pending and confirmed bookings to be "no show" status
		var updated = await dbContext.Bookings
			.Where(b => (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) && b.Start < now)
			.ExecuteUpdateAsync(b => b
				.SetProperty(booking => booking.Status, BookingStatus.NoShow)
				.SetProperty(booking => booking.UpdatedAt, now),
				cancellationToken);

		return Result.Success(updated, $"Updated {updated} pending bookings to NoShow status.");
	}
}

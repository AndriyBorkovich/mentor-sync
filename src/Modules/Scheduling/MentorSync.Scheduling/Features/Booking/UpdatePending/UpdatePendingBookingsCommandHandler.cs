using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.Booking.UpdatePending;

public sealed class UpdatePendingBookingsCommandHandler(SchedulingDbContext schedulingDbContext)
: ICommandHandler<UpdatePendingBookingsCommand, int>
{
	public async Task<Result<int>> Handle(UpdatePendingBookingsCommand request, CancellationToken cancellationToken = default)
	{
		var now = DateTimeOffset.UtcNow;

		// update all pending and confirmed bookings to be noshow status
		var updated = await schedulingDbContext.Bookings
			.Where(b => (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) && b.Start < now)
			.ExecuteUpdateAsync(b => b
				.SetProperty(booking => booking.Status, BookingStatus.NoShow)
				.SetProperty(booking => booking.UpdatedAt, now),
				cancellationToken);

		return Result.Success(updated, $"Updated {updated} pending bookings to NoShow status.");
	}
}

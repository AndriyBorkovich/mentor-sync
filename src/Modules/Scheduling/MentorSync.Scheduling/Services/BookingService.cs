using MentorSync.Scheduling.Contracts.Models;
using MentorSync.Scheduling.Contracts.Services;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Services;

internal sealed class BookingService(SchedulingDbContext schedulingDbContext) : IBookingService
{
	public async Task<IReadOnlyList<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
	{
		return await schedulingDbContext.Bookings
			.AsNoTracking()
			.Where(b => b.Status == BookingStatus.NoShow
				|| b.Status == BookingStatus.Completed
				|| b.Status == BookingStatus.Cancelled)
			.Select(booking => new BookingModel
			{
				MentorId = booking.MentorId,
				MenteeId = booking.MenteeId,
				Status = booking.Status,
			})
			.ToListAsync(cancellationToken);
	}
}

using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.MentorAvailability.Get;

/// <summary>
/// Handler for retrieving mentor availability within a specified date range
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class GetMentorAvailabilityQueryHandler(
	SchedulingDbContext dbContext)
	: IQueryHandler<GetMentorAvailabilityQuery, MentorAvailabilityResult>
{
	/// <inheritdoc />
	public async Task<Result<MentorAvailabilityResult>> Handle(
		GetMentorAvailabilityQuery request,
		CancellationToken cancellationToken = default)
	{
		var startDate = request.StartDate.ToUniversalTime();
		var endDate = request.EndDate.ToUniversalTime();

		var availabilitySlots = await dbContext.MentorAvailabilities
			.Where(a => a.MentorId == request.MentorId && a.Start >= startDate && a.End <= endDate)
			.ToListAsync(cancellationToken);

		// Get all bookings in the date range to check which slots are already booked
		var bookings = await dbContext.Bookings
			.Where(b => b.MentorId == request.MentorId && b.Status != BookingStatus.Cancelled
					&& b.Start >= startDate && b.End <= endDate)
			.ToListAsync(cancellationToken);

		var slots = availabilitySlots
			.ConvertAll(a => new AvailabilitySlot(
				a.Id,
				a.Start,
				a.End,
				// Check if this slot is overlapping with any booking
				bookings.Exists(b => b.Start <= a.End && b.End >= a.Start)))
;

		return new MentorAvailabilityResult(request.MentorId, slots);
	}
}

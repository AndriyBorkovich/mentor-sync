using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.Booking.Create;

/// <summary>
/// Handler for creating a new booking
/// </summary>
/// <param name="dbContext">Database contet</param>
public sealed class CreateBookingCommandHandler(
	SchedulingDbContext dbContext)
	: ICommandHandler<CreateBookingCommand, CreateBookingResult>
{
	/// <inheritdoc />
	public async Task<Result<CreateBookingResult>> Handle(
		CreateBookingCommand request,
		CancellationToken cancellationToken = default)
	{
		// Check if the availability slot exists
		var availabilitySlot = await dbContext.MentorAvailabilities
			.FirstOrDefaultAsync(a => a.Id == request.AvailabilitySlotId, cancellationToken);

		if (availabilitySlot == null)
		{
			return Result.NotFound("Availability slot not found");
		}

		// Validate that the availability belongs to the specified mentor
		if (availabilitySlot.MentorId != request.MentorId)
		{
			return Result.Invalid(
				new ValidationError("Availability slot does not belong to the specified mentor"));
		}

		// Check if the time slot is within the mentor's availability
		if (request.Start < availabilitySlot.Start || request.End > availabilitySlot.End)
		{
			return Result.Invalid(
				new ValidationError("Booking time is outside the mentor's available time slot"));
		}

		// Check if the slot is already booked
		var existingBooking = await dbContext.Bookings
			.Where(b => b.MentorId == request.MentorId && b.Status != BookingStatus.Cancelled && b.Start < request.End && b.End > request.Start)
			.AnyAsync(cancellationToken);

		if (existingBooking)
		{
			return Result.Invalid(
				new ValidationError("This time slot is already booked"));
		}

		// Create the booking
		var booking = new Domain.Booking
		{
			MentorId = request.MentorId,
			MenteeId = request.MenteeId,
			Start = request.Start,
			End = request.End,
			Status = BookingStatus.Pending,
			CreatedAt = DateTimeOffset.UtcNow,
			UpdatedAt = DateTimeOffset.UtcNow,
		};

		dbContext.Bookings.Add(booking);
		await dbContext.SaveChangesAsync(cancellationToken);

		return new CreateBookingResult(
			booking.Id,
			booking.MentorId,
			booking.MenteeId,
			booking.Start,
			booking.End,
			booking.Status.ToString());
	}
}

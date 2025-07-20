using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel.CommonEntities.Enums;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.Booking.Create;

public sealed class CreateBookingCommandHandler(
    SchedulingDbContext dbContext)
    : IRequestHandler<CreateBookingCommand, Result<CreateBookingResult>>
{
    public async Task<Result<CreateBookingResult>> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
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
            .Where(b => b.MentorId == request.MentorId)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b => b.Start < request.End && b.End > request.Start)
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
            UpdatedAt = DateTimeOffset.UtcNow
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

using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.MentorAvailability.Get;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.MentorAvailability;

public sealed class GetMentorAvailabilityQueryHandler(
    SchedulingDbContext dbContext)
    : IRequestHandler<GetMentorAvailabilityQuery, Result<MentorAvailabilityResult>>
{
    public async Task<Result<MentorAvailabilityResult>> Handle(
        GetMentorAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        var startDate = request.StartDate.ToUniversalTime();
        var endDate = request.EndDate.ToUniversalTime();
        // Get all availability slots in the date range
        var availabilitySlots = await dbContext.MentorAvailabilities
            .Where(a => a.MentorId == request.MentorId)
            .Where(a => a.Start >= startDate && a.End <= endDate)
            .ToListAsync(cancellationToken);

        // Get all bookings in the date range to check which slots are already booked
        var bookings = await dbContext.Bookings
            .Where(b => b.MentorId == request.MentorId)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b => b.Start >= startDate && b.End <= endDate)
            .ToListAsync(cancellationToken);

        // Map availabilities to AvailabilitySlot objects, marking those that are booked
        var slots = availabilitySlots
            .Select(a => new AvailabilitySlot(
                a.Id,
                a.Start,
                a.End,
                // Check if this slot is booked (overlapping with any booking)
                bookings.Any(b => b.Start <= a.End && b.End >= a.Start)))
            .ToList();

        return new MentorAvailabilityResult(request.MentorId, slots);
    }
}

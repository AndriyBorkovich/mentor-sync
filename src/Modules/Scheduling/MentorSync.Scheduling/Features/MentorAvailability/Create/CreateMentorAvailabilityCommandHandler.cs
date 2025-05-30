using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.MentorAvailability.Create;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.MentorAvailability;

public sealed class CreateMentorAvailabilityCommandHandler(
    SchedulingDbContext dbContext)
    : IRequestHandler<CreateMentorAvailabilityCommand, Result<CreateMentorAvailabilityResult>>
{
    public async Task<Result<CreateMentorAvailabilityResult>> Handle(
        CreateMentorAvailabilityCommand request,
        CancellationToken cancellationToken)
    {
        // Validate time period
        if (request.Start >= request.End)
        {
            return Result.Invalid(new ValidationError(
                "Invalid time range: Start time must be before end time"));
        }

        // Check for overlapping availability slots
        var overlappingSlots = await dbContext.MentorAvailabilities
            .Where(a => a.MentorId == request.MentorId)
            .Where(a => a.Start < request.End && a.End > request.Start)
            .AnyAsync(cancellationToken);

        if (overlappingSlots)
        {
            return Result.Invalid(new ValidationError(
                "Overlapping availability slot: This time slot overlaps with an existing availability slot"));
        }

        // Create new availability slot
        var availability = new Domain.MentorAvailability
        {
            MentorId = request.MentorId,
            Start = request.Start,
            End = request.End
        };

        dbContext.MentorAvailabilities.Add(availability);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateMentorAvailabilityResult(
            availability.Id,
            availability.MentorId,
            availability.Start,
            availability.End);
    }
}

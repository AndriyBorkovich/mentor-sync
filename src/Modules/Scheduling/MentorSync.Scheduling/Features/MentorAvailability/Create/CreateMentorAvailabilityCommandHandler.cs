using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;
using MentorAvailabilityEntity = MentorSync.Scheduling.Domain.MentorAvailability;
namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

public sealed class CreateMentorAvailabilityCommandHandler(
	SchedulingDbContext dbContext)
	: ICommandHandler<CreateMentorAvailabilityCommand, CreateMentorAvailabilityResult>
{
	public async Task<Result<CreateMentorAvailabilityResult>> Handle(
		CreateMentorAvailabilityCommand request,
		CancellationToken cancellationToken = default)
	{
		// Validate time period
		if (request.Start >= request.End)
		{
			return Result.Invalid(new ValidationError(
				"Invalid time range: Start time must be before end time"));
		}

		// Check for overlapping availability slots
		var hasOverlappingSlots = await dbContext.MentorAvailabilities
			.Where(a => a.MentorId == request.MentorId)
			.Where(a => a.Start < request.End && a.End > request.Start)
			.AnyAsync(cancellationToken);

		if (hasOverlappingSlots)
		{
			return Result.Invalid(new ValidationError(
				"Overlapping availability slot: This time slot overlaps with an existing availability slot"));
		}

		var availability = new MentorAvailabilityEntity
		{
			MentorId = request.MentorId,
			Start = request.Start,
			End = request.End,
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

using Ardalis.Result;
using MentorSync.Scheduling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

public sealed class DeleteMentorAvailabilityCommandHandler(
	SchedulingDbContext dbContext,
	ILogger<DeleteMentorAvailabilityCommandHandler> logger)
		: ICommandHandler<DeleteMentorAvailabilityCommand, string>
{

	public async Task<Result<string>> Handle(DeleteMentorAvailabilityCommand request, CancellationToken cancellationToken = default)
	{
		var availability = await dbContext.MentorAvailabilities
			.FirstOrDefaultAsync(a => a.Id == request.AvailabilityId && a.MentorId == request.MentorId, cancellationToken);

		if (availability == null)
		{
			logger.LogWarning("Mentor availability with ID {AvailabilityId} for mentor {MentorId} not found.", request.AvailabilityId, request.MentorId);
			return Result.NotFound($"Mentor availability with ID {request.AvailabilityId} not found.");
		}

		// handle removal of the bookings associated with this availability if needed

		// For example, if you want to delete bookings, you can do it here
		// start and end collision
		var bookings = await dbContext.Bookings
			.Where(b => b.Start == availability.Start && b.End == availability.End && b.MentorId == request.MentorId)
			.ToListAsync(cancellationToken);

		if (bookings.Count != 0)
		{
			dbContext.Bookings.RemoveRange(bookings);
			logger.LogInformation("Removed {BookingCount} bookings associated with availability ID {AvailabilityId} for mentor {MentorId}.", bookings.Count, request.AvailabilityId, request.MentorId);
		}

		dbContext.MentorAvailabilities.Remove(availability);
		await dbContext.SaveChangesAsync(cancellationToken);

		logger.LogInformation("Deleted mentor availability with ID {AvailabilityId} for mentor {MentorId}.", request.AvailabilityId, request.MentorId);
		return Result.Success("Availability slot was deleted successfully");
	}
}

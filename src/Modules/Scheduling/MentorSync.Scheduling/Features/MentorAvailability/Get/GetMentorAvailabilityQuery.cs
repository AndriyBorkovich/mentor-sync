namespace MentorSync.Scheduling.Features.MentorAvailability.Get;

/// <summary>
/// Query to get mentor availability within a specified date range
/// </summary>
/// <param name="MentorId">Mentor identifier</param>
/// <param name="StartDate"></param>
/// <param name="EndDate"></param>
public sealed record GetMentorAvailabilityQuery(
	int MentorId,
	DateTimeOffset StartDate,
	DateTimeOffset EndDate) : IQuery<MentorAvailabilityResult>;

/// <summary>
/// Result containing mentor availability slots
/// </summary>
/// <param name="MentorId">Mentor identifier</param>
/// <param name="Slots">Slots</param>
public sealed record MentorAvailabilityResult(
	int MentorId,
	IReadOnlyList<AvailabilitySlot> Slots);

/// <summary>
/// Represents a time slot for mentor availability
/// </summary>
/// <param name="Id">The unique identifier of the availability slot.</param>
/// <param name="Start">The start time of the availability slot.</param>
/// <param name="End">The end time of the availability slot.</param>
/// <param name="IsBooked">Indicates whether the slot is booked.</param>
public sealed record AvailabilitySlot(
	int Id,
	DateTimeOffset Start,
	DateTimeOffset End,
	bool IsBooked);

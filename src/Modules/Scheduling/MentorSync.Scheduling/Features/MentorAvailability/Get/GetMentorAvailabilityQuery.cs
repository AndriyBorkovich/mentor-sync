namespace MentorSync.Scheduling.Features.MentorAvailability.Get;

public sealed record GetMentorAvailabilityQuery(
	int MentorId,
	DateTimeOffset StartDate,
	DateTimeOffset EndDate) : IQuery<MentorAvailabilityResult>;

public sealed record MentorAvailabilityResult(
	int MentorId,
	IReadOnlyList<AvailabilitySlot> Slots);

public sealed record AvailabilitySlot(
	int Id,
	DateTimeOffset Start,
	DateTimeOffset End,
	bool IsBooked);

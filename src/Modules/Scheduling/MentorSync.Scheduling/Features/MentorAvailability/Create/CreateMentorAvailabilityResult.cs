namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

/// <summary>
/// Result of creating a mentor availability
/// </summary>
public sealed record CreateMentorAvailabilityResult(
	int Id,
	int MentorId,
	DateTimeOffset Start,
	DateTimeOffset End);

namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

/// <summary>
/// Command to create a new mentor availability slot
/// </summary>
public sealed record CreateMentorAvailabilityCommand(
	int MentorId,
	DateTimeOffset Start,
	DateTimeOffset End) : ICommand<CreateMentorAvailabilityResult>;

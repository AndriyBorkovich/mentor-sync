namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

public sealed record CreateMentorAvailabilityRequest(DateTimeOffset Start, DateTimeOffset End);

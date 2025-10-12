namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

/// <summary>
/// Request model for creating mentor availability
/// </summary>
public sealed record CreateMentorAvailabilityRequest(DateTimeOffset Start, DateTimeOffset End);

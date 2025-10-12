namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

/// <summary>
/// Command to delete a mentor's availability
/// </summary>
public sealed record DeleteMentorAvailabilityCommand(int MentorId, int AvailabilityId) : ICommand<string>;

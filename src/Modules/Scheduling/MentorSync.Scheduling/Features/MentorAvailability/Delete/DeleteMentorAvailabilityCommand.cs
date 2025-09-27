namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

public sealed record DeleteMentorAvailabilityCommand(int MentorId, int AvailabilityId) : ICommand<string>;

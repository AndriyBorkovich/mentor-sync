namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

public record DeleteMentorAvailabilityCommand(int MentorId, int AvailabilityId) : ICommand<string>;

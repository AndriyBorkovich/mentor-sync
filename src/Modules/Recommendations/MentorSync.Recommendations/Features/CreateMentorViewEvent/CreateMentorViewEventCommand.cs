namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

public sealed record CreateMentorViewEventCommand(int MenteeId, int MentorId) : ICommand<string>;

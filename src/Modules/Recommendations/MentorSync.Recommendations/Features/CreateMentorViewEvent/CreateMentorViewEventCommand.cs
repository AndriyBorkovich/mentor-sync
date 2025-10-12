namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

/// <summary>
/// Command to create a mentor view event
/// </summary>
/// <param name="MenteeId">Identifier of the mentee viewing the mentor</param>
/// <param name="MentorId">Identifier of the mentor being viewed</param>
public sealed record CreateMentorViewEventCommand(int MenteeId, int MentorId) : ICommand<string>;

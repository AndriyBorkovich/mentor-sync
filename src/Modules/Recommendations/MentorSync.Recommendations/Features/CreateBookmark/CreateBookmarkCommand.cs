namespace MentorSync.Recommendations.Features.CreateBookmark;

/// <summary>
/// Command to create a bookmark between a mentee and a mentor
/// </summary>
/// <param name="MenteeId">Mentee Id</param>
/// <param name="MentorId">Mentor Id</param>
public sealed record CreateBookmarkCommand(int MenteeId, int MentorId) : ICommand<string>;

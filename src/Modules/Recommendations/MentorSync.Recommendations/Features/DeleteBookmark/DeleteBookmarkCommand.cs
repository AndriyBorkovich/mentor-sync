namespace MentorSync.Recommendations.Features.DeleteBookmark;

/// <summary>
/// Command to delete a bookmark between a mentee and a mentor
/// </summary>
/// <param name="MenteeId">Mentee Id</param>
/// <param name="MentorId">Mentor Id</param>
public sealed record DeleteBookmarkCommand(int MenteeId, int MentorId) : ICommand<string>;

namespace MentorSync.Recommendations.Features.DeleteBookmark;

public sealed record DeleteBookmarkCommand(int MenteeId, int MentorId) : ICommand<string>;

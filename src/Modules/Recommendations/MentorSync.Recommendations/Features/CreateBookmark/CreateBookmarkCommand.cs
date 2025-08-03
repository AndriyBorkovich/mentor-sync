namespace MentorSync.Recommendations.Features.CreateBookmark;

public sealed record CreateBookmarkCommand(
	int MenteeId,
	int MentorId) : ICommand<string>;

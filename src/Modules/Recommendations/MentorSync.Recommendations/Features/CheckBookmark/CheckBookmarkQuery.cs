namespace MentorSync.Recommendations.Features.CheckBookmark;

public sealed record CheckBookmarkQuery(
	int MenteeId,
	int MentorId) : IQuery<CheckBookmarkResult>;

public sealed record CheckBookmarkResult(bool IsBookmarked);

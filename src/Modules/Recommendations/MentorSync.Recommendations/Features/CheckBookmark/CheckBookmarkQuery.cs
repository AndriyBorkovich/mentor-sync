namespace MentorSync.Recommendations.Features.CheckBookmark;

/// <summary>
/// Query to check if a bookmark exists between a mentee and a mentor.
/// </summary>
public sealed record CheckBookmarkQuery(
	int MenteeId,
	int MentorId) : IQuery<CheckBookmarkResult>;

using Ardalis.Result;
using MentorSync.Recommendations.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.CheckBookmark;

/// <summary>
/// Handler for checking if a mentor is bookmarked by a mentee
/// </summary>
/// <param name="dbContext"></param>
public sealed class CheckBookmarkQueryHandler(
	RecommendationsDbContext dbContext)
	: IQueryHandler<CheckBookmarkQuery, CheckBookmarkResult>
{
	/// <inheritdoc />
	public async Task<Result<CheckBookmarkResult>> Handle(
		CheckBookmarkQuery request,
		CancellationToken cancellationToken = default)
	{
		var bookmarkExists = await dbContext.MentorBookmarks
			.AnyAsync(b =>
				b.MenteeId == request.MenteeId &&
				b.MentorId == request.MentorId,
				cancellationToken);

		return new CheckBookmarkResult(bookmarkExists);
	}
}

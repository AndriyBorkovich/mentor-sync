using Ardalis.Result;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Tracking;

namespace MentorSync.Recommendations.Features.CreateBookmark;

/// <summary>
/// Handler for creating a bookmark
/// </summary>
/// <param name="dbContext"></param>
public sealed class CreateBookmarkCommandHandler(
	RecommendationsDbContext dbContext)
		: ICommandHandler<CreateBookmarkCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(
		CreateBookmarkCommand request, CancellationToken cancellationToken = default)
	{
		var bookmark = new MentorBookmark
		{
			MentorId = request.MentorId,
			MenteeId = request.MenteeId,
			BookmarkedAt = DateTime.UtcNow,
		};

		dbContext.MentorBookmarks.Add(bookmark);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success("Bookmark created successfully");
	}
}

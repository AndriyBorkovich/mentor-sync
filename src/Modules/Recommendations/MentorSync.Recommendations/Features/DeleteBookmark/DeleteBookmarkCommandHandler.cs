using Ardalis.Result;
using MentorSync.Recommendations.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.DeleteBookmark;

/// <summary>
/// Handler for deleting a bookmark
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class DeleteBookmarkCommandHandler(
	RecommendationsDbContext dbContext) : ICommandHandler<DeleteBookmarkCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken = default)
	{
		var bookmark = await dbContext.MentorBookmarks
			.FirstOrDefaultAsync(b => b.MentorId == request.MentorId && b.MenteeId == request.MenteeId, cancellationToken);

		if (bookmark == null)
		{
			return Result.NotFound($"Bookmark not found.");
		}

		dbContext.MentorBookmarks.Remove(bookmark);

		await dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success("Bookmark deleted successfully");
	}
}

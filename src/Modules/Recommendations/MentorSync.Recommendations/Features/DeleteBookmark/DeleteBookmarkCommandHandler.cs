using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.DeleteBookmark;

public sealed class DeleteBookmarkCommandHandler(
    RecommendationsDbContext dbContext) : IRequestHandler<DeleteBookmarkCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken)
    {
        var bookmark = await dbContext.MentorBookmarks
            .FirstOrDefaultAsync(b => b.MentorId == request.MentorId && b.MenteeId == request.MenteeId, cancellationToken);
        if (bookmark == null)
        {
            return Result.NotFound($"Bookmark not found.");
        }

        dbContext.MentorBookmarks.Remove(bookmark);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

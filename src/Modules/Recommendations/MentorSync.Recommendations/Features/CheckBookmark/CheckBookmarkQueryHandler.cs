using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.CheckBookmark;

public sealed class CheckBookmarkQueryHandler(
    RecommendationsDbContext dbContext)
    : IRequestHandler<CheckBookmarkQuery, Result<CheckBookmarkResult>>
{
    public async Task<Result<CheckBookmarkResult>> Handle(
        CheckBookmarkQuery request,
        CancellationToken cancellationToken)
    {
        var bookmarkExists = await dbContext.MentorBookmarks
            .AnyAsync(b =>
                b.MenteeId == request.MenteeId &&
                b.MentorId == request.MentorId,
                cancellationToken);

        return new CheckBookmarkResult(bookmarkExists);
    }
}

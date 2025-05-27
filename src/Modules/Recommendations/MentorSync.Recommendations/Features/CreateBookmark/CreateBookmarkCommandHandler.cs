using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Tracking;

namespace MentorSync.Recommendations.Features.CreateBookmark;

public sealed class CreateBookmarkCommandHandler(
    RecommendationsDbContext dbContext)
        : IRequestHandler<CreateBookmarkCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateBookmarkCommand request, CancellationToken cancellationToken)
    {
        var bookmark = new MentorBookmark
        {
            MentorId = request.MentorId,
            MenteeId = request.MenteeId,
            BookmarkedAt = DateTime.UtcNow
        };

        dbContext.MentorBookmarks.Add(bookmark);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

using Ardalis.Result;
using MediatR;

namespace MentorSync.Recommendations.Features.CheckBookmark;

public sealed record CheckBookmarkQuery(
    int MenteeId,
    int MentorId) : IRequest<Result<CheckBookmarkResult>>;

public sealed record CheckBookmarkResult(bool IsBookmarked);

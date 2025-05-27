using Ardalis.Result;
using MediatR;

namespace MentorSync.Recommendations.Features.CreateBookmark;

public sealed record CreateBookmarkCommand(
    int MenteeId,
    int MentorId) : IRequest<Result<Unit>>;

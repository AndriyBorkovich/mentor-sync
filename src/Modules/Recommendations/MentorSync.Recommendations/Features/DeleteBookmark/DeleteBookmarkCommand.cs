using Ardalis.Result;
using MediatR;

namespace MentorSync.Recommendations.Features.DeleteBookmark;

public record DeleteBookmarkCommand(int MenteeId,
    int MentorId) : IRequest<Result<Unit>>;

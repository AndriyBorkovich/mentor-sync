using Ardalis.Result;
using MediatR;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

public sealed record CreateMentorViewEventCommand(int MenteeId, int MentorId) : IRequest<Result<Unit>>;

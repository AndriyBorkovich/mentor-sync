using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

public sealed record CreateMentorAvailabilityCommand(
    int MentorId,
    DateTimeOffset Start,
    DateTimeOffset End) : IRequest<Result<CreateMentorAvailabilityResult>>;

public sealed record CreateMentorAvailabilityResult(
    int Id,
    int MentorId,
    DateTimeOffset Start,
    DateTimeOffset End);

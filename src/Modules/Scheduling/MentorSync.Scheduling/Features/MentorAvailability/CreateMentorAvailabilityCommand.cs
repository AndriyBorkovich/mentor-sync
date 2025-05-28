using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.MentorAvailability;

public sealed record CreateMentorAvailabilityCommand(
    int MentorId,
    DateTimeOffset Start,
    DateTimeOffset End) : IRequest<Result<CreateMentorAvailabilityResult>>;

public sealed record CreateMentorAvailabilityResult(
    int Id,
    int MentorId,
    DateTimeOffset Start,
    DateTimeOffset End);

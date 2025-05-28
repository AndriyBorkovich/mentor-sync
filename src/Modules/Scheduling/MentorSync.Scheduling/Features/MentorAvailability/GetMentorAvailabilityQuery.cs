using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.MentorAvailability;

public sealed record GetMentorAvailabilityQuery(
    int MentorId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate) : IRequest<Result<MentorAvailabilityResult>>;

public sealed record MentorAvailabilityResult(
    int MentorId,
    List<AvailabilitySlot> Slots);

public sealed record AvailabilitySlot(
    int Id,
    DateTimeOffset Start,
    DateTimeOffset End,
    bool IsBooked);

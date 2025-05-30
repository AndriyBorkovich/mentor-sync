using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking.Create;

public sealed record CreateBookingCommand(
    int MenteeId,
    int MentorId,
    int AvailabilitySlotId,
    DateTimeOffset Start,
    DateTimeOffset End) : IRequest<Result<CreateBookingResult>>;

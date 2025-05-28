using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking;

public sealed record CreateBookingCommand(
    int MenteeId,
    int MentorId,
    int AvailabilitySlotId,
    DateTimeOffset Start,
    DateTimeOffset End) : IRequest<Result<CreateBookingResult>>;

public sealed record CreateBookingResult(
    int BookingId,
    int MentorId,
    int MenteeId,
    DateTimeOffset Start,
    DateTimeOffset End,
    string Status);

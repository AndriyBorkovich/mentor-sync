using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking;

public sealed record CancelBookingCommand(int BookingId, int UserId) : IRequest<Result<string>>;

using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking;

public sealed record ConfirmBookingCommand(int BookingId) : IRequest<Result<string>>;

using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.Booking.UpdatePending;

public sealed record UpdatePendingBookingsCommand() : IRequest<Result<int>>;


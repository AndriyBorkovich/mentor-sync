using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.GetUserBookings;

public sealed record GetMentorBookingsQuery(int MentorId) : IRequest<Result<UserBookingsResponse>>;

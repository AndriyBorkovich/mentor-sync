using Ardalis.Result;
using MediatR;

namespace MentorSync.Scheduling.Features.GetUserBookings;

public sealed record GetMenteeBookingsQuery(int MenteeId) : IRequest<Result<UserBookingsResponse>>;

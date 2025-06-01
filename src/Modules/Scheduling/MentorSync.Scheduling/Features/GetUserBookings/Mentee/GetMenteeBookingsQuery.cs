using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentee;

public sealed record GetMenteeBookingsQuery(int MenteeId) : IRequest<Result<UserBookingsResponse>>;

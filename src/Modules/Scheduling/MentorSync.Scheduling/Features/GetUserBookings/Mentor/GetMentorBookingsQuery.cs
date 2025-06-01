using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

public sealed record GetMentorBookingsQuery(int MentorId) : IRequest<Result<UserBookingsResponse>>;

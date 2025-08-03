using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentee;

public sealed record GetMenteeBookingsQuery(int MenteeId) : IQuery<UserBookingsResponse>;

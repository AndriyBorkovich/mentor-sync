using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

public sealed record GetMentorBookingsQuery(int MentorId) : IQuery<UserBookingsResponse>;

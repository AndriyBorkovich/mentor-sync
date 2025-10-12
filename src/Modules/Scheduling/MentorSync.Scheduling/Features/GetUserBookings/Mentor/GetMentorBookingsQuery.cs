using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

/// <summary>
/// Query to get bookings for a specific mentor
/// </summary>
public sealed record GetMentorBookingsQuery(int MentorId) : IQuery<UserBookingsResponse>;

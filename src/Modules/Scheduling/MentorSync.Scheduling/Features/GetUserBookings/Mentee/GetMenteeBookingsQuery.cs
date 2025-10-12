using MentorSync.Scheduling.Features.GetUserBookings.Common;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentee;

/// <summary>
/// Query to get bookings for a specific mentee
/// </summary>
public sealed record GetMenteeBookingsQuery(int MenteeId) : IQuery<UserBookingsResponse>;

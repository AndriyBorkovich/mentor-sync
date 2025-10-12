namespace MentorSync.Scheduling.Features.GetUserBookings.Common;

/// <summary>
/// Response containing a list of bookings for a user
/// </summary>
public sealed class UserBookingsResponse
{
	/// <summary>
	/// List of bookings associated with the user
	/// </summary>
	public IReadOnlyList<BookingDto> Bookings { get; set; } = [];
}

namespace MentorSync.SharedKernel.CommonEntities.Enums;

/// <summary>
/// Represents the status of a booking between mentor and mentee
/// </summary>
public enum BookingStatus
{
	/// <summary>
	/// Booking is pending confirmation
	/// </summary>
	Pending,

	/// <summary>
	/// Booking has been confirmed by both parties
	/// </summary>
	Confirmed,

	/// <summary>
	/// Booking has been cancelled
	/// </summary>
	Cancelled,

	/// <summary>
	/// Booking has been completed successfully
	/// </summary>
	Completed,

	/// <summary>
	/// Mentor or mentee did not show up for the meeting
	/// </summary>
	NoShow
}

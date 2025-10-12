namespace MentorSync.Scheduling.Contracts.Models;

/// <summary>
/// Model representing a booking between a mentor and a mentee
/// </summary>
public sealed class BookingModel
{
	/// <summary>
	/// Unique identifier for the booking
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// Unique identifier for the mentee
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Status of the booking
	/// </summary>
	public BookingStatus Status { get; set; }
}

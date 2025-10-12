namespace MentorSync.Scheduling.Domain;

/// <summary>
/// Represents a booking between a mentor and a mentee
/// </summary>
public sealed class Booking
{
	/// <summary>
	/// Unique identifier for the booking
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Identifier of the mentor involved in the booking
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// Identifier of the mentee involved in the booking
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Start time of the booking
	/// </summary>
	public DateTimeOffset Start { get; set; }
	/// <summary>
	/// End time of the booking
	/// </summary>
	public DateTimeOffset End { get; set; }
	/// <summary>
	/// Current status of the booking
	/// </summary>
	public BookingStatus Status { get; set; }
	/// <summary>
	/// Timestamp when the booking was created
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Timestamp when the booking was last updated
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}


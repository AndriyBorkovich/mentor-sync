namespace MentorSync.Scheduling.Features.GetUserBookings.Common;

/// <summary>
/// Data Transfer Object representing a booking
/// </summary>
public sealed class BookingDto
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
	/// Name of the mentor involved in the booking
	/// </summary>
	public string MentorName { get; set; } = string.Empty;
	/// <summary>
	/// Image URL of the mentor involved in the booking
	/// </summary>
	public string MentorImage { get; set; }
	/// <summary>
	/// Identifier of the mentee involved in the booking
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// Name of the mentee involved in the booking
	/// </summary>
	public string MenteeName { get; set; } = string.Empty;
	/// <summary>
	/// Image URL of the mentee involved in the booking
	/// </summary>
	public string MenteeImage { get; set; }
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
	public string Status { get; set; } = string.Empty;
	/// <summary>
	/// Timestamp when the booking was created
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Timestamp when the booking was last updated
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}

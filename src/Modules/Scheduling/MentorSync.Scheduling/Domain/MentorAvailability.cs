namespace MentorSync.Scheduling.Domain;

/// <summary>
/// Represents an available time slot of a mentor.
/// </summary>
public sealed class MentorAvailability
{
	/// <summary>
	/// Unique identifier for the mentor availability slot.
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Identifier of the mentor associated with this availability.
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// Start time of the availability slot.
	/// </summary>
	public DateTimeOffset Start { get; set; }
	/// <summary>
	/// End time of the availability slot.
	/// </summary>
	public DateTimeOffset End { get; set; }
}

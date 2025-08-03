namespace MentorSync.Scheduling.Domain;

/// <summary>
/// Represents an available time slot of a mentor.
/// </summary>
public sealed class MentorAvailability
{
	public int Id { get; set; }
	public int MentorId { get; set; }
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
}

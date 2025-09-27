namespace MentorSync.Scheduling.Contracts.Models;

public sealed class BookingModel
{
	public int MentorId { get; set; }
	public int MenteeId { get; set; }
	public BookingStatus Status { get; set; }
}

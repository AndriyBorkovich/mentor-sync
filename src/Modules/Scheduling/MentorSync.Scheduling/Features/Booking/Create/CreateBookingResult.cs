namespace MentorSync.Scheduling.Features.Booking.Create;

/// <summary>
/// Result of creating a booking
/// </summary>
public sealed record CreateBookingResult(
	int BookingId,
	int MentorId,
	int MenteeId,
	DateTimeOffset Start,
	DateTimeOffset End,
	string Status);

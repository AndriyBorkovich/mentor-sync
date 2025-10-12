namespace MentorSync.Scheduling.Features.Booking.Create;

/// <summary>
/// Request to create a new booking
/// </summary>
public sealed record CreateBookingRequest(int MentorId, int AvailabilitySlotId, DateTimeOffset Start, DateTimeOffset End);

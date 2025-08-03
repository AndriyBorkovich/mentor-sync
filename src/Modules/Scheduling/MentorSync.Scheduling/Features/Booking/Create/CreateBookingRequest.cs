namespace MentorSync.Scheduling.Features.Booking.Create;

public sealed record CreateBookingRequest(int MentorId, int AvailabilitySlotId, DateTimeOffset Start, DateTimeOffset End);

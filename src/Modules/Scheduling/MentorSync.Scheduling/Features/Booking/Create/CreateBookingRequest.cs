namespace MentorSync.Scheduling.Features.Booking.Create;

public record CreateBookingRequest(int MentorId, int AvailabilitySlotId, DateTimeOffset Start, DateTimeOffset End);

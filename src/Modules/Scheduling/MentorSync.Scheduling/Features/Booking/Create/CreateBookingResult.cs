namespace MentorSync.Scheduling.Features.Booking.Create;

public sealed record CreateBookingResult(
    int BookingId,
    int MentorId,
    int MenteeId,
    DateTimeOffset Start,
    DateTimeOffset End,
    string Status);

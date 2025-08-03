namespace MentorSync.Scheduling.Features.Booking.Cancel;

public sealed record CancelBookingCommand(int BookingId, int UserId) : ICommand<string>;

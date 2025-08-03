namespace MentorSync.Scheduling.Features.Booking.Confirm;

public sealed record ConfirmBookingCommand(int BookingId) : ICommand<string>;

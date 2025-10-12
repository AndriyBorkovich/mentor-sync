namespace MentorSync.Scheduling.Features.Booking.Confirm;

/// <summary>
/// Command to confirm a booking
/// </summary>
public sealed record ConfirmBookingCommand(int BookingId) : ICommand<string>;

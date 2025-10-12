namespace MentorSync.Scheduling.Features.Booking.Cancel;

/// <summary>
/// Command to cancel a booking
/// </summary>
public sealed record CancelBookingCommand(int BookingId, int UserId) : ICommand<string>;

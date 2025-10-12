namespace MentorSync.Scheduling.Features.Booking.UpdatePending;

/// <summary>
/// Command to update pending bookings
/// </summary>
public sealed record UpdatePendingBookingsCommand : ICommand<int>;

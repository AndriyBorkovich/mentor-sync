using MentorSync.Scheduling.Contracts.Models;

namespace MentorSync.Scheduling.Contracts.Services;

/// <summary>
/// Service for managing bookings
/// </summary>
public interface IBookingService
{
	/// <summary>
	/// Gets all bookings
	/// </summary>
	/// <param name="cancellationToken">Token to cancel operation</param>
	/// <returns>List of all bookings</returns>
	Task<IReadOnlyList<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
}

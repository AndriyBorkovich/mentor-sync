using MentorSync.Scheduling.Contracts.Models;

namespace MentorSync.Scheduling.Contracts.Services;

public interface IBookingService
{
	Task<IReadOnlyList<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
}

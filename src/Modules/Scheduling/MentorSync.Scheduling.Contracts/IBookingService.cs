using MentorSync.Scheduling.Contracts.Models;

namespace MentorSync.Scheduling.Contracts;

public interface IBookingService
{
	Task<IReadOnlyList<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
}

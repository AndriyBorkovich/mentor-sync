using MentorSync.Scheduling.Contracts.Models;

namespace MentorSync.Scheduling.Contracts;

public interface IBookingService
{
    Task<List<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default);
}

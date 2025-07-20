using MentorSync.Scheduling.Contracts;
using MentorSync.Scheduling.Contracts.Models;
using MentorSync.Scheduling.Data;
using MentorSync.SharedKernel.CommonEntities.Enums;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Services;

internal sealed class BookingService(SchedulingDbContext schedulingDbContext) : IBookingService
{
    public async Task<List<BookingModel>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
    {
        var result = await schedulingDbContext.Bookings
            .AsNoTracking()
            .Where(b => b.Status == BookingStatus.NoShow
                || b.Status == BookingStatus.Completed
                || b.Status == BookingStatus.Cancelled)
            .Select(booking => new BookingModel
            {
                MentorId = booking.MentorId,
                MenteeId = booking.MenteeId,
                Status = booking.Status
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}

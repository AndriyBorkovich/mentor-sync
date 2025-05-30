using Ardalis.Result;
using MediatR;
using MentorSync.Scheduling.Data;
using MentorSync.Users.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.GetMentorBookings;

public sealed class GetMentorBookingsQueryHandler(
    SchedulingDbContext dbContext,
    IMenteeProfileService userProfileService)
    : IRequestHandler<GetMentorBookingsQuery, Result<MentorBookingsResult>>
{
    public async Task<Result<MentorBookingsResult>> Handle(
        GetMentorBookingsQuery request,
        CancellationToken cancellationToken)
    {
        // Get all bookings for this mentor in the date range
        var bookings = await dbContext.Bookings
            .Where(b => b.MentorId == request.MentorId)
            .Where(b => b.Start >= request.StartDate && b.End <= request.EndDate)
            .ToListAsync(cancellationToken);

        // Extract mentee IDs to fetch their details
        var menteeIds = bookings.Select(b => b.MenteeId).Distinct().ToList();

        // Get basic info for all mentees involved
        var menteeProfiles = new Dictionary<int, (string Name, string Image)>();
        foreach (var menteeId in menteeIds)
        {
            try
            {
                var profile = await userProfileService.GetMenteeInfo(menteeId);
                menteeProfiles[menteeId] = (profile.UserName, profile.ProfileImageUrl);
            }
            catch
            {
                // If we can't get the mentee profile, use placeholder values
                menteeProfiles[menteeId] = ("Unknown User", "/assets/avatars/default.jpg");
            }
        }

        // Map domain entities to DTOs
        var bookingDtos = bookings.Select(b => new BookingDto(
            b.Id,
            b.MentorId,
            b.MenteeId,
            menteeProfiles.TryGetValue(b.MenteeId, out var profile) ? profile.Name : "Unknown User",
            menteeProfiles.TryGetValue(b.MenteeId, out var image) ? image.Image : "/assets/avatars/default.jpg",
            b.Start,
            b.End,
            $"Session with {(menteeProfiles.TryGetValue(b.MenteeId, out var title) ? title.Name : "a mentee")}",
            "Mentoring session",
            b.Status.ToString()
        )).ToList();

        return new MentorBookingsResult(request.MentorId, bookingDtos);
    }
}

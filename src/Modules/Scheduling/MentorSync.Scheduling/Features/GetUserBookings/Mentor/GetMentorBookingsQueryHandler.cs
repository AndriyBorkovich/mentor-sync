using Ardalis.Result;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

public sealed class GetMentorBookingsQueryHandler(
	SchedulingDbContext dbContext)
	: IQueryHandler<GetMentorBookingsQuery, UserBookingsResponse>
{
	public async Task<Result<UserBookingsResponse>> Handle(GetMentorBookingsQuery request, CancellationToken cancellationToken = default)
	{
		// Use raw SQL to fetch bookings with mentor and mentee names from Users table

		var bookings = await dbContext.Database
			.SqlQuery<BookingDto>(
				$"""
                SELECT 
                b."Id",
                b."MentorId",
                mentor."UserName" AS "MentorName",
                mentor."ProfileImageUrl" AS "MentorImage",
                b."MenteeId",
                mentee."UserName" AS "MenteeName",
                mentee."ProfileImageUrl" AS "MenteeImage",
                b."Start",
                b."End",
                b."Status",
                b."CreatedAt",
                b."UpdatedAt"
                FROM scheduling."Bookings" b
                INNER JOIN users."Users" mentor ON b."MentorId" = mentor."Id"
                INNER JOIN users."Users" mentee ON b."MenteeId" = mentee."Id"
                WHERE b."MentorId" = {request.MentorId}
                ORDER BY b."Start" DESC
            """)
			.ToListAsync(cancellationToken);

		var response = new UserBookingsResponse
		{
			Bookings = bookings,
		};

		return Result.Success(response);
	}
}

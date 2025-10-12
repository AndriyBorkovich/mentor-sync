using Ardalis.Result;
using MentorSync.Scheduling.Data;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentee;

/// <summary>
/// Query handler to get bookings for a specific mentee
/// </summary>
/// <param name="dbContext"></param>
public sealed class GetMenteeBookingsQueryHandler(
	SchedulingDbContext dbContext)
	: IQueryHandler<GetMenteeBookingsQuery, UserBookingsResponse>
{
	/// <inheritdoc />
	public async Task<Result<UserBookingsResponse>> Handle(GetMenteeBookingsQuery request, CancellationToken cancellationToken = default)
	{
		var bookings = await dbContext.Database.SqlQuery<BookingDto>(
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
            WHERE b."MenteeId" = {request.MenteeId}
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

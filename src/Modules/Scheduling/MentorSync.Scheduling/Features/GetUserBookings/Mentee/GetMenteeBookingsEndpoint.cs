using System.Security.Claims;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentee;

/// <summary>
/// Endpoint to get all bookings for a mentee
/// </summary>
public sealed class GetMenteeBookingsEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scheduling/mentee/bookings", async (
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var result = await mediator.SendQueryAsync<GetMenteeBookingsQuery, UserBookingsResponse>(new(menteeId), ct);

			return result.DecideWhatToReturn();
		})
		.WithTags("Scheduling")
		.WithDescription("Gets all bookings for a mentee")
		.Produces<UserBookingsResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

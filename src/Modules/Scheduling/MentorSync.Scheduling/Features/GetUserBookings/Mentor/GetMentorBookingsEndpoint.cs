using System.Security.Claims;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

/// <summary>
/// Endpoint to get all bookings for a mentor
/// </summary>
public sealed class GetMentorBookingsEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scheduling/mentor/bookings", async (
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var mentorId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var result = await mediator.SendQueryAsync<GetMentorBookingsQuery, UserBookingsResponse>(new(mentorId), ct);

			return result.DecideWhatToReturn();
		})
		.WithTags("Scheduling")
		.WithDescription("Gets all bookings for a mentor")
		.Produces<UserBookingsResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}

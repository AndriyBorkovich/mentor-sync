using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.Booking.Cancel;

public sealed class CancelBookingEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("scheduling/bookings/{bookingId}/cancel", async (
			int bookingId,
			IMediator mediator,
			HttpContext context,
			CancellationToken ct) =>
		{
			var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var result = await mediator.SendCommandAsync<CancelBookingCommand, string>(new(bookingId, userId), ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Scheduling)
		.WithDescription("Cancels a booking")
		.Produces<string>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly);
	}
}

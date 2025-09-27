using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.Booking.Confirm;

public sealed class ConfirmBookingEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("scheduling/bookings/{bookingId:int}/confirm", async (
			int bookingId,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<ConfirmBookingCommand, string>(new(bookingId), ct);

			return result.DecideWhatToReturn();
		})
		.WithTags("Scheduling")
		.WithDescription("Confirms a pending booking")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}

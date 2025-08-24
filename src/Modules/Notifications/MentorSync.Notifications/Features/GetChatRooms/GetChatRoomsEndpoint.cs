using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetChatRooms;

public sealed class GetChatRoomsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("chat/rooms", async (
			IMediator mediator,
			HttpContext httpContext,
			CancellationToken cancellationToken) =>
			{
				var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
				{
					return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
				}

				var result = await mediator.SendQueryAsync<GetChatRoomsQuery, List<GetChatRoomsResponse>>(new GetChatRoomsQuery(userId), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Notifications)
			.Produces<List<GetChatRoomsResponse>>()
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.RequireAuthorization();
	}
}

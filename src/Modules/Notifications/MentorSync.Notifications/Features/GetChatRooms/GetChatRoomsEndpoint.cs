using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetChatRooms;

/// <summary>
/// Endpoint for retrieving chat rooms associated with the authenticated user
/// </summary>
public sealed class GetChatRoomsEndpoint : IEndpoint
{
	/// <inheritdoc />
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

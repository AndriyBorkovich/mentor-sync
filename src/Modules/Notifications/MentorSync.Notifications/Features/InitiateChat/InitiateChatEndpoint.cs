using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.InitiateChat;

/// <summary>
/// Endpoint for initiating a chat between users
/// </summary>
public sealed class InitiateChatEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("chat/initiate", async (
			InitiateChatRequest request,
			IMediator mediator,
			HttpContext httpContext,
			CancellationToken cancellationToken) =>
			{
				var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
				{
					return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
				}

				var result = await mediator.SendCommandAsync<InitiateChatCommand, InitiateChatResponse>(new InitiateChatCommand(userId, request.RecipientId), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Notifications)
			.Produces<InitiateChatResponse>()
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.RequireAuthorization();
	}
}

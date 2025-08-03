using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetChatMessages;

public sealed class GetChatMessagesEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("chat/messages/{roomId}", async (string roomId, IMediator mediator, HttpContext httpContext) =>
			{
				var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
				{
					return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
				}

				var result = await mediator.SendQueryAsync<GetChatMessagesQuery, List<GetChatMessagesResponse>>(new GetChatMessagesQuery(roomId, userId));

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Notifications)
			.WithDescription("Get chat messages for a specific room")
			.Produces<List<GetChatMessagesResponse>>()
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.RequireAuthorization();
	}
}

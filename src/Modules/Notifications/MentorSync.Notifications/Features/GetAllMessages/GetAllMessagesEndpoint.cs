using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetAllMessages;

/// <summary>
/// Endpoint for retrieving all notification messages
/// </summary>
public sealed class GetAllMessagesEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("notifications", async (IMediator mediator, CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetAllMessagesQuery, List<GetAllMessagesResponse>>(new (), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Notifications)
			.Produces<List<GetAllMessagesResponse>>()
			.Produces(StatusCodes.Status204NoContent)
			.RequireAuthorization(PolicyConstants.AdminOnly);
	}
}

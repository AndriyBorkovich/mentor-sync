using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetAllMessages;

public sealed class GetAllMessagesEndpoint : IEndpoint
{
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

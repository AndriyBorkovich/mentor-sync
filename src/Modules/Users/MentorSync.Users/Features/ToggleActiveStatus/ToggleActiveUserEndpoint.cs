using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ToggleActiveStatus;

/// <summary>
/// Endpoint to toggle a user's active status
/// </summary>
public sealed class ToggleActiveUserEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/{userId:int}/active", async (
				int userId,
				IMediator mediator,
				CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<ToggleActiveUserCommand, string>(new (userId), cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Activates/deactivates specified user")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminOnly);
	}
}

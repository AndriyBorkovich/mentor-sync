using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.DeleteAvatar;

/// <summary>
/// Endpoint to delete user profile image
/// </summary>
public sealed class DeleteAvatarEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapDelete("/users/{id:int}/avatar", async (
				int id,
				IMediator mediator,
				CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<DeleteAvatarCommand, string>(new DeleteAvatarCommand(id), cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Delete user profile image")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly)
		.DisableAntiforgery();
	}
}

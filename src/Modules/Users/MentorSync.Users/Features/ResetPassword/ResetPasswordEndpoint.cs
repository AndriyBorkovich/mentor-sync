using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ResetPassword;

/// <summary>
/// Endpoint to reset a user's password
/// </summary>
public sealed class ResetPasswordEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/reset-password", async (
			ResetPasswordCommand command,
			IMediator mediator,
			CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<ResetPasswordCommand, string>(command, cancellationToken);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Reset password for user")
		.AllowAnonymous()
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.ProducesProblem(StatusCodes.Status409Conflict);
	}
}

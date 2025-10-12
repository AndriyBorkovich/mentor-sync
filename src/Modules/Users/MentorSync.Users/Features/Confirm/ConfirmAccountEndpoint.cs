using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Confirm;

/// <summary>
/// Endpoint to confirm a user account using email and token
/// </summary>
public sealed class ConfirmAccountEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/users/confirm", async (
				[FromQuery] string email,
				[FromQuery] string token,
				IMediator mediator,
				CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<ConfirmAccountCommand, string>(new ConfirmAccountCommand(email, token), cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Confirm user account by sending email with token")
		.AllowAnonymous()
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status400BadRequest);
	}
}

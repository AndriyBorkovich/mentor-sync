using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Login;

/// <summary>
/// Endpoint to login a user
/// </summary>
public sealed class LoginEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/login", async (
				LoginCommand command,
				IMediator mediator,
				CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(command, cancellationToken);
				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Users)
			.WithDescription("Login user with its credentials")
			.AllowAnonymous()
			.Produces<AuthResponse>()
			.Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
	}
}

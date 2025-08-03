using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Login;

public sealed class LoginEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/login", async (LoginCommand command, IMediator mediator) =>
			{
				var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(command);
				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Users)
			.WithDescription("Login user with its credentials")
			.AllowAnonymous()
			.Produces<AuthResponse>(StatusCodes.Status200OK)
			.Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
	}
}

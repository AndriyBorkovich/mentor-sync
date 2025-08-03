using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Refresh;

public sealed class RefreshTokenEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/refresh-token", async (
			RefreshTokenCommand command,
			IMediator mediator) =>
		{
			var result = await mediator.SendCommandAsync<RefreshTokenCommand, AuthResponse>(command);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.AllowAnonymous()
		.Produces<AuthResponse>(StatusCodes.Status200OK)
		.Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
	}
}

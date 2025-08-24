using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Confirm;

public sealed class ConfirmAccountEndpoint : IEndpoint
{
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
		.Produces<string>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest);
	}
}

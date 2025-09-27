using MentorSync.SharedKernel;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Register;

/// <summary>
/// Endpoint for handling user registration requests.
/// </summary>
/// <remarks>
/// This endpoint maps a POST request to 'users/register' and allows anonymous access.
/// It processes registration requests and returns ID of the created user upon successful registration.
/// </remarks>
public sealed class RegisterEndpoint : IEndpoint
{
	/// <summary>
	/// Maps the registration endpoint to the application's routing configuration.
	/// </summary>
	/// <param name="app">The endpoint route builder used to configure the API endpoint.</param>
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("users/register", async (
				RegisterCommand request,
				IMediator mediator,
				CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<RegisterCommand, CreatedEntityResponse>(request, cancellationToken);

			return result.DecideWhatToReturn();
		})
		.AllowAnonymous()
		.WithTags(TagsConstants.Users)
		.WithDescription("Register new user")
		.Produces<CreatedEntityResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status409Conflict);
	}
}

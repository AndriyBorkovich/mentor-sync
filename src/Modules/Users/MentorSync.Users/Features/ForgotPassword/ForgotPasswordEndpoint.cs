using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ForgotPassword;

/// <summary>
/// Endpoint for initiating the forgot password process for a user by sending a reset email.
/// </summary>
/// <example>
/// <code>
/// POST /users/forgot-password
/// {
///     "email": "user@example.com"
/// }
/// </code>
/// </example>
public sealed class ForgotPasswordEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/forgot-password", async (
			[FromBody] string email,
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
			var result = await mediator.SendCommandAsync<ForgotPasswordCommand, string>(new ForgotPasswordCommand(email, baseUrl), ct);

			return result.DecideWhatToReturn();
		})
		.AllowAnonymous()
		.WithTags(TagsConstants.Users)
		.WithDescription("Initiate forgot password process for user (send email)")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.ProducesProblem(StatusCodes.Status409Conflict);
	}
}

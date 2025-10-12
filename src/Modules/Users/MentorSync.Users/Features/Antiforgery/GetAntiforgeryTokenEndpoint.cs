using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Antiforgery;

/// <summary>
/// Endpoint to get an antiforgery token
/// </summary>
public sealed class GetAntiforgeryTokenEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("users/antiforgery/token", (HttpContext httpContext, IAntiforgery antiforgery) =>
		{
			var tokens = antiforgery.GetAndStoreTokens(httpContext);

			return Results.Ok(tokens.RequestToken);
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Generate antiforgery token")
		.Produces<string>()
		.RequireAuthorization();
	}
}

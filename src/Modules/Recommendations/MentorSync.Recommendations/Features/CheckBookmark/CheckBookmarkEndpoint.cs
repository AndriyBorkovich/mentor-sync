using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.CheckBookmark;

/// <summary>
/// Endpoint for checking if a mentor is bookmarked by the current mentee
/// </summary>
public sealed class CheckBookmarkEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/recommendations/bookmarks/check/{mentorId:int}", async (
			[FromRoute] int mentorId,
			IMediator mediator,
			HttpContext httpContext,
			CancellationToken cancellationToken) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var query = new CheckBookmarkQuery(menteeId, mentorId);
			var result = await mediator.SendQueryAsync<CheckBookmarkQuery, CheckBookmarkResult>(query, cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Recommendations)
		.WithDescription("Checks if a mentor is bookmarked by the current mentee.")
		.Produces<CheckBookmarkResult>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}

using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.CreateBookmark;

/// <summary>
/// Endpoint for creating a bookmark for a mentor by the current mentee
/// </summary>
public sealed class CreateBookmarkEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/recommendations/bookmark/{mentorId:int}", async (
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

			var result = await mediator.SendCommandAsync<CreateBookmarkCommand, string>(new CreateBookmarkCommand(menteeId, mentorId), cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Recommendations)
		.WithDescription("This endpoint allows a mentee to bookmark a mentor for future reference.")
		.Produces<string>()
		.Produces(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}

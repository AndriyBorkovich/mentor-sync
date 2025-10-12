using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

/// <summary>
/// Endpoint for creating a mentor view event
/// </summary>
public sealed class CreateMentorViewEventEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/recommendations/view-mentor/{mentorId}", async (
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

			var result = await mediator.SendCommandAsync<CreateMentorViewEventCommand, string>(new (menteeId, mentorId), cancellationToken);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Recommendations)
		.WithDescription("Record a mentor view event for the current user.")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

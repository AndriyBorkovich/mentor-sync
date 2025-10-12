using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Check;

/// <summary>
/// Endpoint to check if the current mentee has already reviewed a mentor
/// </summary>
public sealed class CheckMentorReviewEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("ratings/reviews/mentor/{mentorId}/check", async (
			int mentorId,
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var query = new CheckMentorReviewQuery(mentorId, menteeId);
			var result = await mediator.SendQueryAsync<CheckMentorReviewQuery, CheckMentorReviewResponse>(query, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Checks if the current mentee has already reviewed a mentor")
		.Produces<CheckMentorReviewResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

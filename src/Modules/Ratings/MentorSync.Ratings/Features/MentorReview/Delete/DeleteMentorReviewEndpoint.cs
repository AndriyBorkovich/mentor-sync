using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Delete;

/// <summary>
/// Endpoint for deleting a mentor review
/// </summary>
public sealed class DeleteMentorReviewEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapDelete("ratings/reviews/mentor/{reviewId:int}", async (
			int reviewId,
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var command = new DeleteMentorReviewCommand(reviewId, menteeId);
			var result = await mediator.SendCommandAsync<DeleteMentorReviewCommand, string>(command, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Deletes an existing review for a mentor")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

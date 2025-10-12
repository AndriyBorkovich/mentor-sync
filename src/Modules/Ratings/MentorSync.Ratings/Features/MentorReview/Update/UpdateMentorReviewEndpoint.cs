using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Update;

/// <summary>
/// Endpoint to update an existing mentor review
/// </summary>
public sealed class UpdateMentorReviewEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("ratings/reviews/mentor", async (
			HttpContext httpContext,
			IMediator sender,
			UpdateMentorReviewRequest request,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var command = new UpdateMentorReviewCommand(
				request.ReviewId,
				menteeId,
				request.Rating,
				request.ReviewText
			);

			var result = await sender.SendCommandAsync<UpdateMentorReviewCommand, UpdateMentorReviewResponse>(command, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Updates an existing review for a mentor")
		.Produces<UpdateMentorReviewResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.ProducesProblem(StatusCodes.Status403Forbidden)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

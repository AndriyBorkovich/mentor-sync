using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Create;

/// <summary>
/// Endpoint to create a new mentor review
/// </summary>
public sealed class CreateMentorReviewEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("ratings/reviews/mentor", async (
			HttpContext httpContext,
			IMediator mediator,
			CreateMentorReviewRequest request,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
			}

			var command = new CreateMentorReviewCommand(
				request.MentorId,
				menteeId,
				request.Rating,
				request.ReviewText
			);

			var result = await mediator.SendCommandAsync<CreateMentorReviewCommand, CreateMentorReviewResponse>(command, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Creates a new review for a mentor")
		.Produces<CreateMentorReviewResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}

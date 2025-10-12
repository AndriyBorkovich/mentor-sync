using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Get;

/// <summary>
/// Endpoint to get all reviews for a specific mentor
/// </summary>
public sealed class GetMentorReviewsEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("ratings/mentors/{id:int}/reviews", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetMentorReviewsQuery, MentorReviewsResponse>(new (id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Gets reviews for a mentor")
			.Produces<MentorReviewsResponse>()
			.Produces(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}

using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MentorReview.Get;

public sealed class GetMentorReviewsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("ratings/mentors/{id}/reviews", async (
			int id,
			IMediator mediator) =>
			{
				var result = await mediator.SendQueryAsync<GetMentorReviewsQuery, MentorReviewsResponse>(new GetMentorReviewsQuery(id));

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Gets reviews for a mentor")
			.Produces<MentorReviewsResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}

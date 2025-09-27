using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed class GetMaterialReviewsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("ratings/materials/{id:int}/reviews", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator
							.SendQueryAsync<GetMaterialReviewsQuery, MaterialReviewsResponse>(new GetMaterialReviewsQuery(id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Gets reviews for a learning material")
			.Produces<MaterialReviewsResponse>()
			.ProducesProblem(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}

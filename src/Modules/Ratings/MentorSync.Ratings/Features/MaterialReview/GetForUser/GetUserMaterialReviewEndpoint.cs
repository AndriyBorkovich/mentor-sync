using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed class GetUserMaterialReviewEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("ratings/materials/{materialId:int}/user/{userId:int}/review", async (
			int materialId,
			int userId,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var query = new GetUserMaterialReviewQuery(materialId, userId);
				var result = await mediator.SendQueryAsync<GetUserMaterialReviewQuery, UserMaterialReviewResponse>(query, cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Gets a user's review for a specific learning material")
			.Produces<UserMaterialReviewResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
	}
}

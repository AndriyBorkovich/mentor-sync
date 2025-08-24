using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Delete;

public sealed class DeleteMaterialReviewEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapDelete("ratings/materials/reviews/{reviewId:int}", async (
			int reviewId,
			[FromQuery] int userId,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var command = new DeleteMaterialReviewCommand(reviewId, userId);

				var result = await mediator.SendCommandAsync<DeleteMaterialReviewCommand, string>(command, cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Deletes a review for a learning material")
			.Produces<string>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status204NoContent)
			.ProducesProblem(StatusCodes.Status403Forbidden)
			.ProducesProblem(StatusCodes.Status404NotFound);
	}
}

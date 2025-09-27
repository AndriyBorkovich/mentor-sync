using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Create;

public sealed class CreateMaterialReviewEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("ratings/materials/{materialId:int}/reviews", async (
			int materialId,
			[FromBody] CreateMaterialReviewRequest request,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var command = new CreateMaterialReviewCommand(
					materialId,
					request.ReviewerId,
					request.Rating,
					request.ReviewText);

				var result = await mediator.SendCommandAsync<CreateMaterialReviewCommand, CreateMaterialReviewResponse>(command, cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Ratings)
			.WithDescription("Creates a new review for a learning material")
			.Produces<CreateMaterialReviewResponse>()
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status403Forbidden);
	}
}

public sealed record CreateMaterialReviewRequest(int ReviewerId, int Rating, string ReviewText);

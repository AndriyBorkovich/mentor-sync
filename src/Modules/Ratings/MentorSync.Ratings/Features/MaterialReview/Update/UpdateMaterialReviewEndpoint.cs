using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Update;

/// <summary>
/// Endpoint to update an existing material review
/// </summary>
public sealed class UpdateMaterialReviewEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("ratings/reviews/material", async (
			IMediator mediator,
			[FromBody] UpdateMaterialReviewCommand request,
			CancellationToken cancellationToken) =>
		{
			var result = await mediator.SendCommandAsync<UpdateMaterialReviewCommand, string>(request, cancellationToken);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Updates an existing review for a material")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
	}
}

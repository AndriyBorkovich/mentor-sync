using Ardalis.Result;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.MaterialReview.Update;

public sealed class UpdateMaterialReviewEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("ratings/reviews/material", async (
			IMediator mediator,
			[FromBody] UpdateMaterialReviewCommand request,
			CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<UpdateMaterialReviewCommand, string>(request, ct);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Ratings)
		.WithDescription("Updates an existing review for a material")
		.Produces<Result>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
	}
}

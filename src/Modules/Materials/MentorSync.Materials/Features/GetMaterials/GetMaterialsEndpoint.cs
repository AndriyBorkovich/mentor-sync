using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.GetMaterials;

public sealed class GetMaterialsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("materials", async (
			string search,
			[FromQuery] string[] types,
			[FromQuery] string[] tags,
			string sortBy,
			int pageNumber,
			int pageSize,
			IMediator mediator) =>
			{
				var result = await mediator.SendQueryAsync<GetMaterialsQuery, MaterialsResponse>(new (
					search,
					types?.ToList(),
					tags?.ToList(),
					sortBy,
					pageNumber <= 0 ? 1 : pageNumber,
					pageSize <= 0 ? 10 : pageSize
				));

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Materials)
			.WithDescription("Gets learning materials with filtering and pagination")
			.Produces<MaterialsResponse>()
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
	}
}

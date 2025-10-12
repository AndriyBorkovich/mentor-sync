using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.GetMaterialById;

/// <summary>
/// Endpoint to get a specific learning material by ID
/// </summary>
public sealed class GetMaterialByIdEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("materials/{id:int}", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetMaterialByIdQuery, MaterialResponse>(new (id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Materials)
			.WithDescription("Gets a specific learning material by ID")
			.Produces<MaterialResponse>()
			.Produces(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}

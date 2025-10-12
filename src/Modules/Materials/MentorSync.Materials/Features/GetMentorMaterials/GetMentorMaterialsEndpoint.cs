using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.GetMentorMaterials;

/// <summary>
/// Endpoint for getting learning materials provided by a specific mentor
/// </summary>
public sealed class GetMentorMaterialsEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("materials/mentors/{id}/materials", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetMentorMaterialsQuery, MentorMaterialsResponse>(new (id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags("Materials")
			.WithDescription("Gets learning materials provided by a mentor")
			.Produces<MentorMaterialsResponse>()
			.Produces(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}

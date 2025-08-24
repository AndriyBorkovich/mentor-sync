using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.CommonEntities.Enums;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.CreateMaterial;

public sealed class CreateMaterialEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("materials", async (
			CreateMaterialRequest request,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var command = new CreateMaterialCommand
				{
					Title = request.Title,
					Description = request.Description,
					Type = Enum.Parse<MaterialType>(request.Type),
					ContentMarkdown = request.ContentMarkdown,
					MentorId = request.MentorId,
					Tags = request.Tags
				};

				var result = await mediator.SendCommandAsync<CreateMaterialCommand, CreateMaterialResponse>(command, cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Materials)
			.WithDescription("Creates a new learning material")
			.Produces<CreateMaterialResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status403Forbidden)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}

using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Abstractions.Messaging;
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
            IMediator sender,
            HttpContext httpContext) =>
            {
                // Convert request to command
                var command = new CreateMaterialCommand
                {
                    Title = request.Title,
                    Description = request.Description,
                    Type = Enum.Parse<MaterialType>(request.Type),
                    ContentMarkdown = request.ContentMarkdown,
                    MentorId = request.MentorId,
                    Tags = request.Tags
                };

                var result = await sender.SendCommandAsync<CreateMaterialCommand, CreateMaterialResponse>(command);

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Materials)
            .WithDescription("Creates a new learning material")
            .Produces<CreateMaterialResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
    }
}

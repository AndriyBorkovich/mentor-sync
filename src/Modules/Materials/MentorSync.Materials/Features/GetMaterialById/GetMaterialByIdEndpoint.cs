using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.GetMaterialById;

public sealed class GetMaterialByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("materials/{id}", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMaterialByIdQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Materials)
            .WithDescription("Gets a specific learning material by ID")
            .Produces<MaterialResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }
}

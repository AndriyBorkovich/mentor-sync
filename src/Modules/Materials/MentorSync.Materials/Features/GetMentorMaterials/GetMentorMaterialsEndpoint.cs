using MediatR;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.GetMentorMaterials;

public sealed class GetMentorMaterialsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("materials/mentors/{id}/materials", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMentorMaterialsQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags("Materials")
            .WithDescription("Gets learning materials provided by a mentor")
            .Produces<MentorMaterialsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }
}

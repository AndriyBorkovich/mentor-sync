using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.AddTags;

public sealed class AddTagsToMaterialEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("materials/{materialId}/tags", async (
            int materialId,
            AddTagsRequest request,
            ISender sender,
            HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var mentorId))
                {
                    return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
                }

                var command = new AddTagsToMaterialCommand
                {
                    MaterialId = materialId,
                    TagNames = request.TagNames,
                    MentorId = mentorId
                };

                var result = await sender.Send(command);

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Materials)
            .WithDescription("Adds tags to an existing learning material")
            .Produces<AddTagsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

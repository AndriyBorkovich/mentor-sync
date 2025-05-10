using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.DeleteAvatar;

public sealed class DeleteAvatarEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id:int}/avatar", async (
                int id,
                ISender sender) =>
        {
            var result = await sender.Send(new DeleteAvatarCommand(id));

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Users)
        .WithDescription("Delete user profile image")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization()
        .RequireAntiforgeryToken();
    }
}

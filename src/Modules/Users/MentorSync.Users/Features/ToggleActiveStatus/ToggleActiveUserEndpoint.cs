using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ToggleActiveStatus;

public sealed class ToggleActiveUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/{userId:int}/active", async (int userId, ISender sender) =>
        {
            var result = await sender.Send(new ToggleActiveUserCommand(userId));

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Users)
        .WithDescription("Activates/deactivates specified user")
        .Produces<string>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly);
    }
}

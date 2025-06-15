using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.InitiateChat;

public sealed class InitiateChatEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("chat/initiate", async (InitiateChatRequest request, ISender sender, HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
                }

                var result = await sender.Send(new InitiateChatCommand(userId, request.RecipientId));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Notifications)
            .Produces<InitiateChatResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}

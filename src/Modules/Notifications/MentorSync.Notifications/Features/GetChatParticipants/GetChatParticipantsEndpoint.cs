using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetChatParticipants;

public sealed class GetChatParticipantsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("chat/participants", async (ISender sender, HttpContext context) =>
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
                }

                var result = await sender.Send(new GetChatParticipantsQuery(userId));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Notifications)
            .Produces<List<GetChatParticipantsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
    }
}

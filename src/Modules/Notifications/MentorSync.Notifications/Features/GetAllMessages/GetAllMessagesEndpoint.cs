using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Notifications.Features.GetAllMessages;

public sealed class GetAllMessagesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("notifications", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllMessagesQuery());

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Notifications)
            .Produces<List<GetAllMessagesResponse>>()
            .Produces<Unit>(StatusCodes.Status204NoContent)
            .RequireAuthorization();
    }
}

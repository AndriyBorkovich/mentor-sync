using MediatR;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

public sealed class GetMentorUpcomingSessionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("scheduling/mentors/{id}/upcoming-sessions", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMentorUpcomingSessionsQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags("Scheduling")
            .WithDescription("Gets upcoming sessions for a mentor")
            .Produces<MentorUpcomingSessionsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }
}

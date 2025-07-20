using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Get;

public sealed class GetMentorAvailabilityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/scheduling/mentors/{mentorId:int}/availability", async (
            [FromRoute] int mentorId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            ISender sender) =>
        {
            var start = startDate != default ?
                new DateTimeOffset(startDate) :
                DateTimeOffset.UtcNow.Date;

            var end = endDate != default ?
                new DateTimeOffset(endDate).AddDays(1).AddSeconds(-1) :
                start.AddDays(7).AddSeconds(-1);

            var query = new GetMentorAvailabilityQuery(mentorId, start, end);
            var result = await sender.Send(query);

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Scheduling)
        .WithDescription("Gets availability slots for a mentor within a date range")
        .Produces<MentorAvailabilityResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
    }
}

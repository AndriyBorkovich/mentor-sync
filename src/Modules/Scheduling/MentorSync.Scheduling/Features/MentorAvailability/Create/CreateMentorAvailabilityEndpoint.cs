using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

public sealed class CreateMentorAvailabilityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/scheduling/mentors/{mentorId:int}/availability", async (
            [FromRoute] int mentorId,
            [FromBody] CreateMentorAvailabilityRequest request,
            ISender sender,
            HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var command = new CreateMentorAvailabilityCommand(
                mentorId,
                request.Start,
                request.End);

            var result = await sender.Send(command);

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Scheduling)
        .WithDescription("Creates a new availability slot for a mentor")
        .Produces<CreateMentorAvailabilityResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
    }
}

public class CreateMentorAvailabilityRequest
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}

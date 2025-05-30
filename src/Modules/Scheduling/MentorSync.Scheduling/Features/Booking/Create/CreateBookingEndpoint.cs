using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.Booking.Create;

public sealed class CreateBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/scheduling/bookings", async (
            [FromBody] CreateBookingRequest request,
            ISender sender,
            HttpContext httpContext) =>
        {
            // Get the mentee ID from the current user
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var command = new CreateBookingCommand(
                menteeId,
                request.MentorId,
                request.AvailabilitySlotId,
                request.Start,
                request.End);

            var result = await sender.Send(command);
            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Scheduling)
        .WithDescription("Creates a new booking for a mentoring session")
        .Produces<CreateBookingResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

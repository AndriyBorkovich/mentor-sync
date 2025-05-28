using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetUserBookings;

public sealed class GetMenteeBookingsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("scheduling/mentee/bookings", async (
            HttpContext httpContext,
            ISender sender,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await sender.Send(new GetMenteeBookingsQuery(menteeId), ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Scheduling")
        .WithDescription("Gets all bookings for a mentee")
        .Produces<UserBookingsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

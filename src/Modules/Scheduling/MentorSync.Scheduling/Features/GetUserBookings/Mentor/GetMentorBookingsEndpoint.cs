using System.Security.Claims;
using MediatR;
using MentorSync.Scheduling.Features.GetUserBookings.Common;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetUserBookings.Mentor;

public sealed class GetMentorBookingsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("scheduling/mentor/bookings", async (
            HttpContext httpContext,
            ISender sender,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var mentorId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await sender.Send(new GetMentorBookingsQuery(mentorId), ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Scheduling")
        .WithDescription("Gets all bookings for a mentor")
        .Produces<UserBookingsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
    }
}

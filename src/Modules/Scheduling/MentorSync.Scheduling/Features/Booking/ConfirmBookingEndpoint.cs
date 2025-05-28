using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.Booking;

public sealed class ConfirmBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("scheduling/bookings/{bookingId}/confirm", async (
            int bookingId,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new ConfirmBookingCommand(bookingId), ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Scheduling")
        .WithDescription("Confirms a pending booking")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
    }
}

using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.CheckMentorReview;

public sealed class CheckMentorReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ratings/reviews/mentor/{mentorId}/check", async (
            int mentorId,
            HttpContext httpContext,
            ISender sender,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var query = new CheckMentorReviewQuery(mentorId, menteeId);
            var result = await sender.Send(query, ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Ratings")
        .WithDescription("Checks if the current mentee has already reviewed a mentor")
        .Produces<CheckMentorReviewResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

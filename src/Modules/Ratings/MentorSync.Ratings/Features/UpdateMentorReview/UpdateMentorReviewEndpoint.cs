using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.UpdateMentorReview;

public sealed class UpdateMentorReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ratings/reviews/mentor", async (
            HttpContext httpContext,
            ISender sender,
            UpdateMentorReviewRequest request,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var command = new UpdateMentorReviewCommand(
                request.ReviewId,
                menteeId,
                request.Rating,
                request.ReviewText
            );

            var result = await sender.Send(command, ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Ratings")
        .WithDescription("Updates an existing review for a mentor")
        .Produces<UpdateMentorReviewResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

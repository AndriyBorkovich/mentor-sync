using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Ratings.Features.CreateMentorReview;

public sealed class CreateMentorReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("ratings/reviews/mentor", async (
            HttpContext httpContext,
            ISender sender,
            CreateMentorReviewRequest request,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var command = new CreateMentorReviewCommand(
                request.MentorId,
                menteeId,
                request.Rating,
                request.ReviewText
            );

            var result = await sender.Send(command, ct);

            return result.DecideWhatToReturn();
        })
        .WithTags("Ratings")
        .WithDescription("Creates a new review for a mentor")
        .Produces<CreateMentorReviewResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

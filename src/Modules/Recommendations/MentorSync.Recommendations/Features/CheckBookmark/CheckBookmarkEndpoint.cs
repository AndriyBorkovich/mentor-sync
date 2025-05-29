using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.CheckBookmark;

public sealed class CheckBookmarkEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/recommendations/bookmarks/check/{mentorId:int}", async (
            [FromRoute] int mentorId,
            ISender sender,
            HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var menteeId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await sender.Send(new CheckBookmarkQuery(menteeId, mentorId));

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Recommendations)
        .WithDescription("Checks if a mentor is bookmarked by the current mentee.")
        .Produces<CheckBookmarkResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
    }
}

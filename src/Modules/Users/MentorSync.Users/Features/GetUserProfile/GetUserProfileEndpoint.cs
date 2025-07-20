using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace MentorSync.Users.Features.GetUserProfile;

public sealed class GetUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/profile", async (
            HttpContext httpContext,
            ISender sender,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: 401);
            }

            var result = await sender.Send(new GetUserProfileQuery(userId), ct);

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Users)
        .WithDescription("Get user profile information including related mentee/mentor profile")
        .RequireAuthorization(PolicyConstants.ActiveUserOnly)
        .Produces<UserProfileResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

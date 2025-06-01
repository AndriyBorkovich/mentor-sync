using System.Security.Claims;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

public sealed class GetRecommendedMentorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/recommendations/mentors", async (
            [FromQuery] string searchTerm,
            [FromQuery] string[] programmingLanguages,
            [FromQuery] int? industry,
            [FromQuery] int? minExperienceYears,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISender sender,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Problem("User ID not found or invalid", statusCode: 401);
            }

            Industry? industryEnum = industry.HasValue ? (Industry)industry.Value : null;

            var result = await sender.Send(new GetRecommendedMentorsQuery(
                userId,
                searchTerm,
                programmingLanguages?.ToList() ?? [],
                industryEnum,
                minExperienceYears,
                pageNumber ?? 1,
                pageSize ?? 10), ct);

            return result.DecideWhatToReturn();
        }).WithTags(TagsConstants.Recommendations)
        .WithDescription("Get recommended mentors for the currently logged-in mentee with filtering options")
        .Produces<PaginatedList<RecommendedMentorResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
    }
}

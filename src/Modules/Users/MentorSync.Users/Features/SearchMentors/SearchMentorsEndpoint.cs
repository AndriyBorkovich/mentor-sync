using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.CommonEntities.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.SearchMentors;

public sealed class SearchMentorsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/mentors/search", async (
            [FromQuery] string searchTerm,
            [FromQuery] string[] programmingLanguages,
            [FromQuery] int? industry,
            [FromQuery] int? minExperienceYears,
            [FromQuery] double? minRating,
            [FromQuery] double? maxRating,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            ISender sender) =>
            {
                var industryEnum = industry.HasValue ? (Industry?)industry.Value : null;
                var programmingLanguagesList = programmingLanguages?.ToList();

                var result = await sender.Send(new SearchMentorsQuery(
                    searchTerm,
                    programmingLanguagesList,
                    industryEnum,
                    minExperienceYears,
                    minRating,
                    maxRating,
                    pageNumber,
                    pageSize));

                return result;
            })
            .WithTags(TagsConstants.Users)
            .WithDescription("Search mentors with filters")
            .Produces<List<MentorSearchResponse>>(StatusCodes.Status200OK)
            .AllowAnonymous();
    }
}

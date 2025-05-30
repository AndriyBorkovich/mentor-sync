using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
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
            ISender sender) =>
            {
                Industry? industryEnum = industry.HasValue ? (Industry)industry.Value : null;
                var programmingLanguagesList = programmingLanguages?.ToList();

                var result = await sender.Send(new SearchMentorsQuery(
                    searchTerm,
                    programmingLanguagesList,
                    industryEnum,
                    minExperienceYears));

                return result;
            })
            .WithTags(TagsConstants.Users)
            .WithDescription("Search mentors with filters")
            .Produces<List<MentorSearchResponse>>(StatusCodes.Status200OK)
            .AllowAnonymous();
    }
}

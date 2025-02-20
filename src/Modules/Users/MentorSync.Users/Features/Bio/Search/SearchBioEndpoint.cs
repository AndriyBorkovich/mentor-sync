using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Bio.Search;

public class SearchBioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/bio", async ([FromQuery, Required] string text, ISender sender) =>
        {
            var result = await sender.Send(new SearchUsersByBioRequest(text));

            return result.DecideWhatToReturn();
        })
        .RequireAuthorization(policyNames: "GooglePolicy")
        .WithTags(TagsConstants.Users)
        .Produces<List<SearchUserByBioResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

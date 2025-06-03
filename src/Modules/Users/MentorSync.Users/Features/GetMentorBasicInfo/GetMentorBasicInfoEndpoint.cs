using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

public sealed class GetMentorBasicInfoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/mentors/{id}/basic-info", async (
            int id,
            ISender sender) =>
            {
                var result = await sender.Send(new GetMentorBasicInfoQuery(id));

                return result.DecideWhatToReturn();
            })
            .WithTags(TagsConstants.Mentors)
            .WithDescription("Gets basic profile information for a mentor")
            .Produces<MentorBasicInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
    }
}

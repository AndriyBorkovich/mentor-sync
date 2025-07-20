using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

public sealed class CreateMenteeProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/mentees/profile", async (
            [FromBody] CreateMenteeProfileCommand request,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(request, ct);

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Mentees)
        .WithDescription("Create new mentee profile")
        .Produces<MenteeProfileResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
    }
}

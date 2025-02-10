using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Refresh;

public sealed class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/refresh-token", async (
            RefreshTokenCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Users)
        .AllowAnonymous()
        .Produces<AuthResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}
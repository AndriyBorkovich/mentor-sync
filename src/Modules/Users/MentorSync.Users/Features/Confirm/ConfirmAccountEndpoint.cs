using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Confirm;

public class ConfirmAccountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/confirm", async ([FromQuery] string email, [FromQuery] string token, ISender sender) =>
        {
            var result = await sender.Send(new ConfirmAccountCommand(email, token));

            return result.DecideWhatToReturn();
        })
        .WithTags(TagsConstants.Users)
        .AllowAnonymous()
        .Produces<string>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}

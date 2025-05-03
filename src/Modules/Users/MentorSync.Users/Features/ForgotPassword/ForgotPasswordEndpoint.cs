using MediatR;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ForgotPassword;

public sealed class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/forgot-password", async (
            [FromBody] string email,
            HttpContext httpContext,
            ISender sender,
            CancellationToken ct) =>
        {
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            var result = await sender.Send(new ForgotPasswordCommand(email, baseUrl), ct);

            return result.DecideWhatToReturn();
        })
        .AllowAnonymous()
        .WithTags(TagsConstants.Users)
        .WithDescription("Initiate forgot password process for user")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}

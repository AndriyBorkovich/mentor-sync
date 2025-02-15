using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Google.Login;

public sealed class GoogleLoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/google/login", (HttpContext context) =>
            {
                var authProperties = new AuthenticationProperties
                {
                    RedirectUri = "/auth/google/callback",
                    Items = { { "returnUrl", context.Request.Query["returnUrl"].ToString() } }
                };

                return Results.Challenge(authProperties, ["Google"]);
            })
            .WithTags(TagsConstants.Google)
            .AllowAnonymous();
    }
}
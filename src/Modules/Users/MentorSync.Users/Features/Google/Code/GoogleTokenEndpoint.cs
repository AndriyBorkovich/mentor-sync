using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Services.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace MentorSync.Users.Features.Google.Code;

public sealed class GoogleTokenEndpoint : IEndpoint
{
    private const string PkceSessionKey = "PkceSessionKey";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/GoogleOAuth/Tokens",
            async (HttpContext httpContext, IGoogleOAuthService googleOAuthService, IConfiguration configuration, string code) =>
        {
            var codeVerifier = httpContext.Session.GetString(PkceSessionKey);
            if (string.IsNullOrEmpty(codeVerifier))
            {
                return Results.BadRequest("Session expired, please retry login.");
            }

            var tokenResult = await googleOAuthService.ExchangeCodeOnTokenAsync(code, codeVerifier, configuration["GoogleOAuth:RedirectUri"]);

            return Results.Ok(new { tokenResult?.AccessToken, tokenResult?.RefreshToken, tokenResult?.ExpiresIn });
        })
        .WithTags(TagsConstants.Google)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}

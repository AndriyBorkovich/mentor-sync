using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Services;
using MentorSync.Users.Services.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace MentorSync.Users.Features.Google.Redirect;

public class GoogleRedirectEndpoint : IEndpoint
{
    private const string PkceSessionKey = "PkceSessionKey";
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/GoogleOAuth/Redirect",
            (HttpContext httpContext, IGoogleOAuthService googleOAuthService, IConfiguration configuration) =>
        {
            var codeVerifier = Guid.NewGuid().ToString(); // Generate code_verifier
            var codeChallenge = Sha256Helper.ComputeHash(codeVerifier);

            httpContext.Session.SetString(PkceSessionKey, codeVerifier);

            var url = googleOAuthService.GenerateOAuthRequestUrl(
                configuration["GoogleOAuth:EmailScope"], // OAuth scope
                configuration["GoogleOAuth:RedirectUri"],
                codeChallenge
            );

            return Results.Redirect(url);
        })
        .WithTags(TagsConstants.Google);
    }
}

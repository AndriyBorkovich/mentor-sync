using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Services.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Google.Refresh;

public sealed class GoogleRefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/GoogleOAuth/RefreshToken", async (IGoogleOAuthService googleOAuthService, [FromBody] string refreshToken) =>
        {
            var tokenResult = await googleOAuthService.RefreshTokenAsync(refreshToken);
            return Results.Ok(new { tokenResult?.AccessToken });
        })
        .WithTags(TagsConstants.Google);
    }
}

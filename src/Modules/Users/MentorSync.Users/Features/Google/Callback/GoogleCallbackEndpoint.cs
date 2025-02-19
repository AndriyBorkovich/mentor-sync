using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Interfaces;
using MentorSync.Users.Domain;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Google.Callback;

public class GoogleCallbackEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/google/callback", async (
            HttpContext context,
            UserManager<AppUser> userManager,
            IJwtTokenGenerator tokenGenerator,
            ILogger<GoogleCallbackEndpoint> logger) =>
        {
            try
            {
                var authenticateResult = await context.AuthenticateAsync("Google");
                if (!authenticateResult.Succeeded)
                {
                    return Results.Unauthorized();
                }

                var googleUser = authenticateResult.Principal;
                var email = googleUser.FindFirstValue(ClaimTypes.Email);
                var googleId = googleUser.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(email))
                {
                    return Results.Problem("Email not found from Google authentication");
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user is null)
                {
                    user = new AppUser
                    {
                        Email = email,
                        UserName = email,
                        EmailConfirmed = true,
                    };

                    var createResult = await userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        return Results.Problem("Failed to create user account");
                    }

                    var addLoginResult = await userManager.AddLoginAsync(user,
                        new UserLoginInfo("Google", googleId, "Google"));

                    if (!addLoginResult.Succeeded)
                    {
                        return Results.Problem("Failed to add external login info");
                    }
                }

                // Generate JWT token
                var token = await tokenGenerator.GenerateToken(user);

                // You can either:
                // 1. Redirect to a frontend page with the token
                // var returnUrl = authenticateResult.Properties?.Items["returnUrl"];
                // if (!string.IsNullOrEmpty(returnUrl))
                // {
                //     return Results.Redirect($"{returnUrl}?token={token.AccessToken}");
                // }

                // 2. Or return the token directly as JSON
                return Results.Ok(new AuthResponse(token.AccessToken, token.RefreshToken, token.Expiration));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during Google authentication callback");
                return Results.Problem("An error occurred during authentication");
            }
        })
        .WithTags(TagsConstants.Google)
        .Produces<AuthResponse>()
        .AllowAnonymous();
    }
}
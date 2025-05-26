using System.Security.Claims;
using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MentorSync.Users.Features.Refresh;

public sealed class RefreshTokenCommandHandler(
    UserManager<AppUser> userManager,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var principal = jwtTokenService.GetPrincipalFromExpiredToken(command.AccessToken);
        if (principal is null)
        {
            return Result.Conflict("Invalid access token");
        }

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId!);
            
        if (user is null)
        {
            return Result.NotFound("User not found");
        }

        if (user.RefreshToken != command.RefreshToken)
        {
            logger.LogWarning("Invalid refresh token for user {UserId}", userId);
            return Result.Conflict("Invalid refresh token");
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            logger.LogWarning("Expired refresh token for user {UserId}", userId);
            return Result.Error("Refresh token has expired, please re-login");
        }

        var tokenResult = await jwtTokenService.GenerateToken(user);

        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);

        logger.LogInformation("Tokens were refreshed for user {UserId}", userId);

        return Result.Success(new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.Expiration, null));
    }
}

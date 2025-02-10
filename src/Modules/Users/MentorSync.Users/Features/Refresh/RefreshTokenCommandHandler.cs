using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MentorSync.Users.Features.Refresh;

public sealed class RefreshTokenCommandHandler(
    UserManager<AppUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IOptions<JwtOptions> jwtOptions,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var principal = GetPrincipalFromExpiredToken(command.AccessToken);
        if (principal is null)
        {
            return Result.Error("Invalid access token");
        }

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId!);
            
        if (user is null)
        {
            return Result.Error("User not found");
        }

        if (user.RefreshToken != command.RefreshToken)
        {
            logger.LogWarning("Invalid refresh token for user {UserId}", userId);
            return Result.Error("Invalid refresh token");
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            logger.LogWarning("Expired refresh token for user {UserId}", userId);
            return Result.Error("Refresh token has expired");
        }

        var tokenResult = await jwtTokenGenerator.GenerateToken(user);

        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);

        logger.LogInformation("Tokens refreshed for user {UserId}", userId);

        return Result.Success(new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.Expiration));
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // Don't validate lifetime here
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, 
                tokenValidationParameters, 
                out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256, 
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
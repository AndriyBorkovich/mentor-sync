using System.Security.Claims;
using Ardalis.Result;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MentorSync.Users.Features.Refresh;

/// <summary>
/// Handles the refresh token command, validating the provided tokens and issuing new access and refresh tokens if valid.
/// </summary>
/// <param name="userManager">
/// The <see cref="UserManager{TUser}"/> instance for managing user data.
/// </param>
/// <param name="jwtTokenService">
/// The <see cref="IJwtTokenService"/> used for JWT operations.
/// </param>
/// <param name="jwtOptions">
/// The <see cref="JwtOptions"/> configuration for JWT settings.
/// </param>
/// <param name="logger">
/// The <see cref="ILogger{TCategoryName}"/> for logging operations.
/// </param>
/// <remarks>
/// This handler validates the expired access token and refresh token, checks user existence and token validity,
/// and issues new tokens if all checks pass.
/// </remarks>
/// <example>
/// <code>
/// var handler = new RefreshTokenCommandHandler(userManager, jwtTokenService, jwtOptions, logger);
/// var result = await handler.Handle(new RefreshTokenCommand(accessToken, refreshToken));
/// </code>
/// </example>
public sealed class RefreshTokenCommandHandler(
	UserManager<AppUser> userManager,
	IJwtTokenService jwtTokenService,
	IOptions<JwtOptions> jwtOptions,
	ILogger<RefreshTokenCommandHandler> logger)
	: ICommandHandler<RefreshTokenCommand, AuthResponse>
{
	private readonly JwtOptions _jwtOptions = jwtOptions.Value;

	/// <inheritdoc />
	public async Task<Result<AuthResponse>> Handle(
		RefreshTokenCommand command,
		CancellationToken cancellationToken = default)
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

		if (!string.Equals(user.RefreshToken, command.RefreshToken, StringComparison.OrdinalIgnoreCase))
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

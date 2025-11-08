using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MentorSync.Users.Features.Login;

/// <summary>
/// Handles user login commands, authenticating users and generating JWT tokens.
/// </summary>
/// <param name="userManager">The user manager for <see cref="AppUser"/> operations.</param>
/// <param name="signInManager">The sign-in manager for handling authentication.</param>
/// <param name="usersDbContext">The database context for user-related data.</param>
/// <param name="jwtTokenService">The service for generating JWT tokens.</param>
/// <param name="jwtOptions">The JWT configuration options.</param>
/// <param name="logger">The logger instance for logging events.</param>
/// <remarks>
/// This handler validates user credentials, issues access and refresh tokens, and determines onboarding requirements.
/// </remarks>
/// <example>
/// <code>
/// var handler = new LoginCommandHandler(userManager, signInManager, dbContext, jwtTokenService, jwtOptions, logger);
/// var result = await handler.Handle(new LoginCommand("user@email.com", "password"));
/// </code>
/// </example>
public sealed class LoginCommandHandler(
	UserManager<AppUser> userManager,
	SignInManager<AppUser> signInManager,
	UsersDbContext usersDbContext,
	IJwtTokenService jwtTokenService,
	IOptions<JwtOptions> jwtOptions,
	ILogger<LoginCommandHandler> logger)
	: ICommandHandler<LoginCommand, AuthResponse>
{
	/// <inheritdoc />
	public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
	{
		var email = command.Email;
		var user = await userManager.FindByEmailAsync(email);
		if (user is null)
		{
			return Result.NotFound("User not found");
		}

		var passwordCheckResult = await CheckPasswordAsync(user, command.Password);
		if (!passwordCheckResult.IsSuccess)
		{
			return passwordCheckResult;
		}

		var tokenResult = await jwtTokenService.GenerateToken(user);

		user.RefreshToken = tokenResult.RefreshToken;
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
		await userManager.UpdateAsync(user);

		logger.LogInformation("User {UserId} logged in successfully", user.Id);

		var userRoles = await userManager.GetRolesAsync(user);
		var role = userRoles.FirstOrDefault() ?? Roles.Admin;

		var needsOnboarding = role switch
		{
			Roles.Mentee => !await usersDbContext.MenteeProfiles.AnyAsync(mp => mp.MenteeId == user.Id,
				cancellationToken),
			Roles.Mentor => !await usersDbContext.MentorProfiles.AnyAsync(mp => mp.MentorId == user.Id,
				cancellationToken),
			_ => false
		};

		return Result.Success(new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.Expiration, needsOnboarding));
	}

	private async Task<Result> CheckPasswordAsync(AppUser user, string password)
	{
		var result = await signInManager.CheckPasswordSignInAsync(
			user,
			password,
			lockoutOnFailure: false);

		if (!result.Succeeded)
		{
			if (result.IsNotAllowed)
			{
				logger.LogWarning("Login failed: User {UserId} is not allowed to sign in", user.Id);
				return Result.Forbidden("Login is not allowed. Please verify your email");
			}

			logger.LogWarning("Login failed: Invalid password or email for user {UserId}", user.Id);
			return Result.Invalid(new ValidationError("Invalid email or password"));
		}

		return Result.Success();
	}
}

using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Login;

public sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtTokenGenerator jwtTokenGenerator,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            logger.LogWarning("Login failed: User not found for email {Email}", command.Email);
            return Result.Error("Invalid email or password");
        }

        var result = await signInManager.CheckPasswordSignInAsync(
            user,
            command.Password,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            if (result.IsNotAllowed)
            {
                logger.LogWarning("Login failed: User {Email} is not allowed to sign in", command.Email);
                return Result.Error("Login is not allowed. Please verify your email");
            }

            logger.LogWarning("Login failed: Invalid password for user {Email}", command.Email);
            return Result.Error("Invalid email or password");
        }

        var tokenResult = await jwtTokenGenerator.GenerateToken(user);

        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        logger.LogInformation("User {Email} logged in successfully", command.Email);

        return Result.Success(new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.Expiration));
    }
}
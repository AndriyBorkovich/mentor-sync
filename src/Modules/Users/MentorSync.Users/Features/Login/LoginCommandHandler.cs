using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Common.Responses;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MentorSync.Users.Features.Login;

public sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    UsersDbContext usersDbContext,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            logger.LogWarning("Login failed: user not found for email {Email}", command.Email);
            return Result.NotFound("User not found");
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
                return Result.Forbidden("Login is not allowed. Please verify your email");
            }

            logger.LogWarning("Login failed: Invalid password or email for user {UserId}", user.Id);
            return Result.Invalid(new ValidationError("Invalid email or password"));
        }

        var tokenResult = await jwtTokenService.GenerateToken(user);

        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);

        logger.LogInformation("User {Email} logged in successfully", command.Email);

        var userRoles = await userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? Roles.Admin;

        var needsOnboarding = false;
        if (role is Roles.Mentee)
        {
            needsOnboarding = !await usersDbContext.MenteeProfiles
                .AnyAsync(mp => mp.MenteeId == user.Id, cancellationToken);
        }
        else if (role is Roles.Mentor)
        {
            needsOnboarding = !await usersDbContext.MentorProfiles
                .AnyAsync(mp => mp.MentorId == user.Id, cancellationToken);
        }

        return Result.Success(new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.Expiration, needsOnboarding));
    }
}

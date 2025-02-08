using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
        {
            logger.LogWarning("Registration attempt with existing email: {Email}", command.Email);
            return Result.Error("User with this email already exists");
        }
            
        var user = new AppUser
        {
            UserName = command.UserName,
            Email = command.Email,
            EmailConfirmed = false
        };

        var createResult = await userManager.CreateAsync(user, command.Password);
        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user: {Errors}", 
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return Result.Error(string.Join(',', createResult.Errors.Select(e => e.Description)));
        }

        logger.LogInformation("User registered successfully: {Email}", command.Email);
        return Result.Success();
    }
}

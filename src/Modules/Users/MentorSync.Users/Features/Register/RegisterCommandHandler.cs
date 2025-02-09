using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
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

        if (await userManager.Users.AnyAsync(u => u.UserName == command.UserName, cancellationToken))
        {
            return Result.Error("User with this name already exists");
        }

        var appRole = await roleManager.FindByNameAsync(command.Role);
        if (appRole is null)
        {
            logger.LogWarning("Registration attempt with role: {Role}", command.Role);
            
            return Result.NotFound($"Role {command.Role} doesn't exist");
        }
            
        var user = new AppUser
        {
            UserName = command.UserName,
            Email = command.Email,
            EmailConfirmed = false,
        };

        var createResult = await userManager.CreateAsync(user, command.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            logger.LogError("Failed to create user: {Errors}", errors);
            
            return Result.Error(errors);
        }
        
        var roleResult = await userManager.AddToRoleAsync(user, command.Role);
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            logger.LogError("Failed to add user to role: {Errors}", errors);
            
            return Result.Error(errors);
        }

        logger.LogInformation("User registered successfully: {Email}", command.Email);
        
        return Result.Success();
    }
}

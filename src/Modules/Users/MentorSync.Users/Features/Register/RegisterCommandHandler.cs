using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Domain;
using MentorSync.Users.Domain.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    UsersDbContext usersDbContext,
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
        };

        var result = await usersDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await using var transaction = await usersDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Perform operations
                var createResult = await userManager.CreateAsync(user, command.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Error(string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                var roleResult = await userManager.AddToRoleAsync(user, command.Role);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Error(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                await transaction.CommitAsync(cancellationToken);

                user.RaiseDomainEvent(new UserCreatedEvent(user.Id));

                await usersDbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "Transaction failed");
                return Result.CriticalError("Transaction failed");
            }
        });

        return result;
    }
}

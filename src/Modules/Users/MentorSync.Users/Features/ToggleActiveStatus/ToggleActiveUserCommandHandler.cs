using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.ToggleActiveStatus;

public sealed class ToggleActiveUserCommandHandler(
    UsersDbContext usersDbContext,
    ILogger<ToggleActiveUserCommandHandler> logger)
    : IRequestHandler<ToggleActiveUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ToggleActiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.NotFound($"User with ID {request.UserId} not found");
        }

        var result = await usersDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await using var transaction = await usersDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                user.IsActive = !user.IsActive;
                usersDbContext.Update(user);

                user.RaiseDomainEvent(new UserActiveStatusChageEvent(user.Email, user.IsActive));

                await usersDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result.Success($"User was {(user.IsActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "Transaction failed");
                return Result.CriticalError("Deactivation failed");
            }
        });

        return result;
    }
}

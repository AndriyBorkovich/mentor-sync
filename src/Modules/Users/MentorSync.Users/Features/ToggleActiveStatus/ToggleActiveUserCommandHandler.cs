using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.ToggleActiveStatus;

/// <summary>
/// Command handler for toggling a user's active status
/// </summary>
/// <param name="usersDbContext">Database context</param>
/// <param name="logger">Logger</param>
public sealed class ToggleActiveUserCommandHandler(
	UsersDbContext usersDbContext,
	ILogger<ToggleActiveUserCommandHandler> logger)
	: ICommandHandler<ToggleActiveUserCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(ToggleActiveUserCommand request, CancellationToken cancellationToken = default)
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

				user.RaiseDomainEvent(new UserActiveStatusChangedEvent(user.Email, user.IsActive));

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

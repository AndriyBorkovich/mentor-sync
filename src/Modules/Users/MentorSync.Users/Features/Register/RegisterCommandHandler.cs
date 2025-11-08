using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.Register;

/// <summary>
/// Command handler for user registration.
/// </summary>
/// <param name="userManager">Handles user management operations like creation and role assignment</param>
/// <param name="usersDbContext">Access to users in the database</param>
/// <param name="logger">Logger for recording registration events and errors</param>
public sealed class RegisterCommandHandler(
	UserManager<AppUser> userManager,
	UsersDbContext usersDbContext,
	ILogger<RegisterCommandHandler> logger)
	: ICommandHandler<RegisterCommand, CreatedEntityResponse>
{
	/// <inheritdoc />
	public async Task<Result<CreatedEntityResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken = default)
	{
		var email = command.Email;

		var validationError = await ValidateRegistrationAsync(command, cancellationToken);
		if (validationError is not null)
		{
			logger.LogWarning("Registration validation failed for email: {Email}, reason: {Reason}", LoggingExtensions.SanitizeForLogging(email), validationError);
			return Result.Conflict(validationError);
		}

		var user = new AppUser
		{
			UserName = command.UserName,
			Email = email,
		};

		var result = await CreateAsync(user, command, cancellationToken);

		return result;
	}

	/// <summary>
	/// Creates a new user and assigns a role within a transactional context.
	/// </summary>
	/// <param name="user">The user to be created.</param>
	/// <param name="command">The registration command containing user details.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A result containing the created entity response or an error message.</returns>
	private async Task<Result<CreatedEntityResponse>> CreateAsync(AppUser user, RegisterCommand command, CancellationToken cancellationToken)
	{
		return await usersDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
		{
			await using var transaction = await usersDbContext.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				var createResult = await userManager.CreateAsync(user, command.Password);
				if (!createResult.Succeeded)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Error(createResult.GetErrorMessage());
				}

				var roleResult = await userManager.AddToRoleAsync(user, command.Role);
				if (!roleResult.Succeeded)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Error(createResult.GetErrorMessage());
				}

				user.IsActive = true;

				await transaction.CommitAsync(cancellationToken);

				user.RaiseDomainEvent(new UserCreatedEvent(user.Id));

				await usersDbContext.SaveChangesAsync(cancellationToken);

				return Result.Success(new CreatedEntityResponse(user.Id));
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				logger.LogError(ex, "Transaction failed");
				return Result.CriticalError($"Transaction failed: {ex.Message}");
			}
		});
	}

	/// <summary>
	/// Validates registration for duplicate email and username.
	/// </summary>
	/// <param name="command">The registration command.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Error message if validation fails, otherwise null.</returns>
	private async Task<string> ValidateRegistrationAsync(RegisterCommand command, CancellationToken cancellationToken)
	{
		var existingUser = await userManager.FindByEmailAsync(command.Email);
		if (existingUser is not null)
		{
			return "User with this email already exists";
		}

		if (await userManager.Users.AnyAsync(u => u.UserName == command.UserName, cancellationToken))
		{
			return "User with this name already exists";
		}

		return null;
	}
}

using System.Security.Claims;
using MentorSync.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Infrastructure;

/// <summary>
/// Requirement to ensure the user is active
/// </summary>
public sealed class ActiveUserRequirement : IAuthorizationRequirement;

/// <summary>
/// Authorization handler to check if the user is active
/// </summary>
/// <param name="usersDbContext">Database context</param>
public sealed class ActiveUserAuthHandler(UsersDbContext usersDbContext) : AuthorizationHandler<ActiveUserRequirement>
{
	/// <inheritdoc />
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		ActiveUserRequirement requirement)
	{
		var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
		{
			context.Fail();
			return;
		}

		var result = int.TryParse(userId, out var id);

		if (!result)
		{
			context.Fail();
			return;
		}

		var userIsActive = await usersDbContext.Users.AnyAsync(u => u.Id == id && u.IsActive);

		if (!userIsActive)
		{
			context.Fail(new AuthorizationFailureReason(this, "User is deactivated"));
			return;
		}

		context.Succeed(requirement);
	}
}

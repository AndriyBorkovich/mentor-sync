using Ardalis.Result;
using MentorSync.Users.Domain.User;
using MentorSync.Users.MappingExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.GetAllUsers;

public sealed class GetAllUsersQueryHandler(UserManager<AppUser> userManager)
	: IQueryHandler<GetAllUsersQuery, List<UserShortResponse>>
{
	public async Task<Result<List<UserShortResponse>>> Handle(
		GetAllUsersQuery request, CancellationToken cancellationToken = default)
	{
		var query = userManager.Users.AsNoTracking();

		request.Deconstruct(out var filteredRole, out var isActiveFilter);
		if (!string.IsNullOrEmpty(filteredRole))
		{
			query = query.Where(user => user.UserRoles.Any(ur => ur.Role.Name == filteredRole));
		}

		if (isActiveFilter is not null)
		{
			query = query.Where(user => user.IsActive == isActiveFilter);
		}

		var users = await query
							.Include(user => user.UserRoles)
								.ThenInclude(ur => ur.Role)
							.ToListAsync(cancellationToken);

		return Result.Success(users.ConvertAll(u => u.ToUserShortResponse()));
	}
}

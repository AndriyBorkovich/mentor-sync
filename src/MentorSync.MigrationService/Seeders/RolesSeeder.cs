using MentorSync.SharedKernel;
using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.MigrationService.Seeders;

public static class RolesSeeder
{
	public static async Task SeedAsync(IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

		await TryToCreateRoleAsync(Roles.Admin);
		await TryToCreateRoleAsync(Roles.Mentor);
		await TryToCreateRoleAsync(Roles.Mentee);

		return;

		async Task TryToCreateRoleAsync(string roleName)
		{
			var appRole = await roleManager.FindByNameAsync(roleName);

			if (appRole is null)
			{
				appRole = new AppRole
				{
					Name = roleName
				};

				await roleManager.CreateAsync(appRole);
			}
		}
	}
}

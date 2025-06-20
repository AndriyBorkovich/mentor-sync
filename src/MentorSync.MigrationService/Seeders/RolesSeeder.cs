using MentorSync.SharedKernel;
using MentorSync.Users.Domain.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.MigrationService.Seeders;

public static class RolesSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        await CreateRole(Roles.Admin);
        await CreateRole(Roles.Mentor);
        await CreateRole(Roles.Mentee);

        return;

        async Task CreateRole(string roleName)
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

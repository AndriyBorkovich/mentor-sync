using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using MentorSync.Users.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Users;

public static class ModuleRegistration
{
    public static void AddUsersModule(this IServiceCollection services, IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<UsersDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
            });

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                
                options.User.RequireUniqueEmail = true;
                
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultTokenProviders();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });
    }
}
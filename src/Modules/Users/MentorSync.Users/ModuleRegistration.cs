using System.Text;
using FluentValidation;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Data;
using MentorSync.Users.Domain;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MentorSync.Users;

public static class ModuleRegistration
{
    public static void AddUsersModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<UsersDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
            });

        AddIdentity(builder.Services);

        AddCustomAuthorization(builder.Services, builder.Configuration);

        AddEndpoints(builder.Services);
    }

    private static void AddIdentity(IServiceCollection services)
    {
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
            .AddSignInManager()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });
    }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(UsersDbContext).Assembly);
    }

    private static void AddCustomAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        
        var jwtOptions = new JwtOptions();
        configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
        
        services.AddSingleton(Options.Create(jwtOptions));
        
        // jwt & google
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authorization = context.Request.Headers.Authorization.ToString();

                        if (string.IsNullOrEmpty(authorization))
                        {
                            context.NoResult();
                        }
                        else
                        {
                            context.Token = authorization.Replace("Bearer ", string.Empty);
                        }

                        return Task.CompletedTask;
                    },
                };
            })
            /*.AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                options.Scope.Add("email");
                options.Scope.Add("profile");
            })*/;

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    }
}
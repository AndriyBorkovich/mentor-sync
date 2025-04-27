using System.Text;
using FluentValidation;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Role;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace MentorSync.Users;

public static class ModuleRegistration
{
    public static void AddUsersModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<UsersDbContext>(
            connectionName: GeneralConstants.DatabaseName,
            configureSettings: c => c.DisableTracing = true,
            configureDbContextOptions: opt =>
            {
                opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
            });

        AddIdentity(builder.Services);

        AddCustomAuth(builder.Services, builder.Configuration);

        AddEndpoints(builder.Services);

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
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

        services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(2));
        }

    private static void AddEndpoints(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModuleRegistration).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleRegistration).Assembly));
        services.AddEndpoints(typeof(UsersDbContext).Assembly);
    }

    private static void AddCustomAuth(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();
        configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

        services.AddSingleton(Options.Create(jwtOptions));

        services.AddAuthentication(
            opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
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

                        return Task.CompletedTask;
                    },
                };
            });

        // TODO: implement google authentication on frontend
        // .AddGoogle(options =>
        // {
        //     options.ClientId = configuration["Authentication:Google:ClientId"]!;
        //     options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;

        //     options.Events = new OAuthEvents
        //     {
        //         OnRedirectToAuthorizationEndpoint = context =>
        //         {
        //             return Task.CompletedTask;
        //         },
        //         OnCreatingTicket = context =>
        //         {
        //             return Task.CompletedTask;
        //         },
        //         OnRemoteFailure = context =>
        //         {
        //             return Task.CompletedTask;
        //         },
        //         OnTicketReceived = context =>
        //         {
        //             return Task.CompletedTask;
        //         }
        //     };
        // });


        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IAuthorizationHandler, ActiveUserAuthHandler>();

        services.AddAuthorization(o =>
        {
            o.AddPolicy(PolicyConstants.ActiveUserOnly, p =>
                p.AddRequirements(new ActiveUserRequirement()));

            o.AddPolicy(PolicyConstants.AdminOnly, p =>
                p.RequireRole(Roles.Admin));

            o.AddPolicy(PolicyConstants.MentorOnly, p =>
                p.RequireRole(Roles.Mentor));

            o.AddPolicy(PolicyConstants.MenteeOnly, p =>
                p.RequireRole(Roles.Mentee));
        });

    }
}

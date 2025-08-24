using System.Text;
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
using MentorSync.Users.Contracts.Services;
using MentorSync.Users.Services;

namespace MentorSync.Users;

public static class ModuleRegistration
{
	public static void AddUsersModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddIdentity(builder.Services);

		AddCustomAuth(builder.Services, builder.Configuration);

		AddEndpoints(builder.Services);

		AddSessionSupport(builder.Services);

		AddExternalServices(builder.Services);
	}

	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<UsersDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt =>
					opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users)));

	private static void AddIdentity(IServiceCollection services)
	{
		services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
				options.SignIn.RequireConfirmedEmail = true;
				options.SignIn.RequireConfirmedPhoneNumber = false;

				options.Password.RequiredLength = GeneralConstants.MinPasswordLength;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = true;

				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = GeneralConstants.AllowedUserNameCharacters;

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(GeneralConstants.DefaultLockoutTimeInMinutes);
				options.Lockout.MaxFailedAccessAttempts = GeneralConstants.MaxFailedAccessAttempts;
				options.Lockout.AllowedForNewUsers = true;
			})
			.AddSignInManager()
			.AddEntityFrameworkStores<UsersDbContext>()
			.AddDefaultTokenProviders();

		services.Configure<IdentityOptions>(options =>
		{
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(GeneralConstants.DefaultLockoutTimeInMinutes);
			options.Lockout.MaxFailedAccessAttempts = GeneralConstants.MaxFailedAccessAttempts;
			options.Lockout.AllowedForNewUsers = true;
		});

		services.Configure<DataProtectionTokenProviderOptions>(options =>
			options.TokenLifespan = TimeSpan.FromHours(GeneralConstants.ProtectionTokenTimeInHours));
	}

	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;
		services.AddValidators(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	private static void AddCustomAuth(IServiceCollection services, IConfiguration configuration)
	{
		services.AddAntiforgery(options =>
		{
			options.HeaderName = "X-XSRF-TOKEN";
			options.Cookie.Name = "XSRF-TOKEN";
		});

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
						Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var authorization = context.Request.Headers.Authorization.ToString();

						if (string.IsNullOrEmpty(authorization))
						{
							// Try to get access token from query string (for SignalR connections)
							var accessToken = context.Request.Query["access_token"];

							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) &&
								(path.StartsWithSegments("/hubs/chat", StringComparison.OrdinalIgnoreCase) || path.StartsWithSegments("/notificationHub", StringComparison.OrdinalIgnoreCase)))
							{
								context.Token = accessToken;
							}
						}
						else
						{
							// Extract token from Authorization header (Bearer token)
							var token = authorization.Replace("Bearer ", string.Empty, StringComparison.Ordinal);
							if (!string.IsNullOrEmpty(token))
							{
								context.Token = token;
							}
						}

						return Task.CompletedTask;
					},
				};
			});

		services.AddScoped<IJwtTokenService, JwtTokenService>();

		services.AddScoped<IAuthorizationHandler, ActiveUserAuthHandler>();

		services.AddAuthorizationBuilder()
			.AddPolicy(PolicyConstants.ActiveUserOnly, p =>
				p.AddRequirements(new ActiveUserRequirement()))
			.AddPolicy(PolicyConstants.AdminOnly, p =>
				p.RequireRole(Roles.Admin))
			.AddPolicy(PolicyConstants.MentorOnly, p =>
				p.RequireRole(Roles.Mentor))
			.AddPolicy(PolicyConstants.MenteeOnly, p =>
				p.RequireRole(Roles.Mentee))
			.AddPolicy(PolicyConstants.AdminMentorMix, p =>
				p.RequireRole(Roles.Admin, Roles.Mentor))
			.AddPolicy(PolicyConstants.AdminMenteeMix, p =>
				p.RequireRole(Roles.Admin, Roles.Mentee))
			.AddPolicy(PolicyConstants.MentorMenteeMix, p =>
				p.RequireRole(Roles.Mentor, Roles.Mentee));
	}

	private static void AddSessionSupport(IServiceCollection services)
	{
		services.AddDistributedMemoryCache();
		services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(10);
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});
	}

	/// <summary>
	/// Services needed for other modules to work with data from users module
	/// </summary>
	/// <param name="services">extended param</param>
	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<IMentorProfileService, MentorProfileService>();
		services.AddScoped<IMenteeProfileService, MenteeProfileService>();
		services.AddScoped<IUserService, UserService>();
	}
}

using MentorSync.Notifications;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Behaviours;
using MentorSync.SharedKernel.Services;
using MentorSync.Users;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MentorSync.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpointsMetadata(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(SwaggerConfiguration.Configure);

        return services;
    }
    public static IServiceCollection AddCustomSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((_, lc) => lc.ReadFrom.Configuration(configuration));

        return services;
    }
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
                ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
            }
        );

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddCustomCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyNames.All,
                policyConfig => policyConfig.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        return services;
    }

    public static void AddAplicationModules(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddSingleton<IDomainEventsDispatcher, MediatorDomainEventsDispatcher>();
        builder.AddUsersModule();
        builder.AddNotificationsModule();
    }
}

static file class SwaggerConfiguration
{
    private static OpenApiSecurityScheme Scheme => new()
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme,
        },
    };

    public static void Configure(SwaggerGenOptions option)
    {
        option.ResolveConflictingActions(apiDesc => apiDesc.First());
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        option.AddSecurityDefinition(Scheme.Reference.Id, Scheme);
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { Scheme, Array.Empty<string>() },
        });
        option.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
    }
}

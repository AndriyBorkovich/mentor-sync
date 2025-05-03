using MentorSync.Notifications;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Behaviours;
using MentorSync.SharedKernel.Services;
using MentorSync.Users;
using Serilog;
using System.Reflection;

namespace MentorSync.API;

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

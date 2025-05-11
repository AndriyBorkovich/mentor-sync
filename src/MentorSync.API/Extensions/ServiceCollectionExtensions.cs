using MentorSync.Materials;
using MentorSync.Notifications;
using MentorSync.Ratings;
using MentorSync.Recommendations;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Behaviours;
using MentorSync.Users;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

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

    public static IServiceCollection AddGlobalRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext => RateLimitPartition.GetConcurrencyLimiter(
                        partitionKey: GetPartitionKey(httpContext),
                        factory: _ => new ConcurrencyLimiterOptions
                        {
                            PermitLimit = 1,
                            QueueLimit = 5,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        })),
                PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext => RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetPartitionKey(httpContext),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }))
            );

            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString();
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                context.Lease.TryGetMetadata(MetadataName.ReasonPhrase, out var reasonPhrase);
                reasonPhrase ??= "Request limit was reached";

                context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                    .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                    .LogWarning("Rejected request: {EndpointName}, reason: {ReasonPhrase}, retry after: {RetryInSeconds}s", context.HttpContext.Request.Path, reasonPhrase, retryAfter.TotalSeconds);

                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
            };

            static string GetPartitionKey(HttpContext httpContext)
            {
                return httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            }
        });

        return services;
    }

    public static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(opts =>
        {
            var j = opts.SerializerOptions;
            j.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            j.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            j.WriteIndented = true;
            j.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            j.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
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

        builder.AddSharedServices();
        builder.AddUsersModule();
        builder.AddNotificationsModule();
        builder.AddMaterialsModule();
        builder.AddRatingsModule();
        builder.AddRecommendationsModule();
    }
}

static file class SwaggerConfiguration
{
    private static OpenApiSecurityScheme BearerScheme => new()
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
        }
    };

    private static OpenApiSecurityScheme AntiforgeryScheme => new()
    {
        In = ParameterLocation.Header,
        Name = "X-XSRF-TOKEN",
        Type = SecuritySchemeType.ApiKey,
        Description = "Antiforgery token. Get it from /antiforgery/token endpoint and send it here.",
        Reference = new OpenApiReference
        {
            Id = "XSRF-TOKEN",
            Type = ReferenceType.SecurityScheme
        }
    };

    public static void Configure(SwaggerGenOptions option)
    {
        option.ResolveConflictingActions(apiDesc => apiDesc.First());
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "MentorSync API", Version = "v1" });
        option.AddSecurityDefinition(BearerScheme.Reference.Id, BearerScheme);
        option.AddSecurityDefinition(AntiforgeryScheme.Reference.Id, AntiforgeryScheme);
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { BearerScheme, Array.Empty<string>() },
            { AntiforgeryScheme, Array.Empty<string>() }
        });
        option.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
    }
}

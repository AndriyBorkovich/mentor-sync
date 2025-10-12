using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace MentorSync.ServiceDefaults;

/// <summary>
/// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
/// This project should be referenced by each service project in your solution.
/// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
/// </summary>
public static class ServicesExtensions
{
	/// <summary>
	/// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
	/// This method should be called from the host builder in each service project in your solution.
	/// </summary>
	/// <param name="builder"></param>
	/// <typeparam name="TBuilder"></typeparam>
	public static void AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		builder.ConfigureOpenTelemetry();

		builder.AddDefaultHealthChecks();

		builder.Services.AddServiceDiscovery();

		builder.Services.ConfigureHttpClientDefaults(http =>
		{
			// Turn on resilience by default
			http.AddStandardResilienceHandler();

			// Turn on service discovery by default
			http.AddServiceDiscovery();
		});

		// Uncomment the following to restrict the allowed schemes for service discovery.
		// builder.Services.Configure<ServiceDiscoveryOptions>(options =>
		// {
		//     options.AllowedSchemes = ["https"];
		// });
	}

	private static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
		where TBuilder : IHostApplicationBuilder
	{
		builder.Logging.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes = true;
			logging.ParseStateValues = true;
		});

		builder.Services.AddOpenTelemetry()
			.WithMetrics(metrics =>
			{
				metrics.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation()
					.AddRuntimeInstrumentation();
			})
			.WithTracing(tracing =>
			{
				tracing.AddSource(builder.Environment.ApplicationName)
					.AddAspNetCoreInstrumentation(t =>
						t.Filter = httpContext => !(httpContext.Request.Path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase)
						                              || httpContext.Request.Path.StartsWithSegments("/alive", StringComparison.OrdinalIgnoreCase)))
					.AddHttpClientInstrumentation()
					.AddEntityFrameworkCoreInstrumentation()
					.AddNpgsql()
					.AddElasticsearchClientInstrumentation()
					.SetSampler(new AlwaysOnSampler());
			});

		builder.AddOpenTelemetryExporters();
	}

	private static void AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
		where TBuilder : IHostApplicationBuilder
	{
		var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

		if (!useOtlpExporter)
		{
			return;
		}

		builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
		builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
		builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

		// Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
		//if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
		//{
		//    builder.Services.AddOpenTelemetry()
		//       .UseAzureMonitor();
		//}
	}

	private static void AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
		where TBuilder : IHostApplicationBuilder
	{
		builder.Services.AddHealthChecks()
			// Add a default liveness check to ensure app is responsive
			.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
	}

	/// <summary>
	/// Maps default endpoints for health checks
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static void MapDefaultEndpoints(this WebApplication app)
	{
		// Adding health checks endpoints to applications in non-development environments has security implications.
		// See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
		if (app?.Environment.IsDevelopment() != true)
		{
			return;
		}

		// All health checks must pass for app to be considered ready to accept traffic after starting
		app.MapHealthChecks("/health");

		// Only health checks tagged with the "live" tag must pass for app to be considered alive
		app.MapHealthChecks("/alive", new HealthCheckOptions
		{
			Predicate = r => r.Tags.Contains("live")
		});
	}
}

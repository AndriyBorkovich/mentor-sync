using MentorSync.Notifications.Infrastructure.Hubs;
using MentorSync.SharedKernel;
using Serilog;

namespace MentorSync.API.Extensions;

/// <summary>
/// Extension methods for configuring the application builder pipeline
/// </summary>
public static class AppBuilderExtensions
{
	/// <summary>
	/// Configures authentication and authorization middleware
	/// </summary>
	/// <param name="app">The application builder instance</param>
	public static void UseAuth(this IApplicationBuilder app)
	{
		app.UseAuthentication();
		app.UseAuthorization();
	}

	/// <summary>
	/// Configures Swagger UI middleware for API documentation
	/// </summary>
	/// <param name="app">The application builder instance</param>
	public static void UseCustomSwagger(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	/// <summary>
	/// Configures Serilog request logging middleware with enrichment
	/// </summary>
	/// <param name="app">The application builder instance</param>
	/// <example>
	/// <code>
	/// app.UseCustomSerilog();
	/// </code>
	/// </example>
	public static void UseCustomSerilog(this IApplicationBuilder app)
	{
		app.UseSerilogRequestLogging(options =>
		{
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			{
				diagnosticContext.Set("RequestPath", httpContext.Request.Path);
				diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
				diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
				diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? string.Empty);
				diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
			};
		});
	}

	/// <summary>
	/// Configures CORS policy middleware
	/// </summary>
	/// <param name="app">The application builder instance</param>
	public static void UseCustomCorsPolicy(this IApplicationBuilder app)
	{
		app.UseCors(CorsPolicyNames.All);
	}

	/// <summary>
	/// Maps SignalR hubs to their endpoints
	/// </summary>
	/// <param name="app">The web application instance</param>
	public static void MapHubs(this WebApplication app)
	{
		app.MapHub<NotificationHub>("/notificationHub");
	}
}

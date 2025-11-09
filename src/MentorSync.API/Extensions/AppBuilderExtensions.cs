using System.Text;
using MentorSync.Notifications.Infrastructure.Hubs;
using Serilog;

namespace MentorSync.API.Extensions;

/// <summary>
/// Extension methods for configuring the application builder pipeline
/// </summary>
internal static class AppBuilderExtensions
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
		UseBodyReader(app);

		const string requestBodyItemKey = "RequestBody";
		app.UseSerilogRequestLogging(options =>
		{
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			{
				diagnosticContext.Set("RequestPath", httpContext.Request.Path);
				diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
				diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
				diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? string.Empty);
				diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);

				if (httpContext.Items.TryGetValue(requestBodyItemKey, out var rb) && rb is string body)
				{
					diagnosticContext.Set("RequestBody", body);
				}
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

	private static void UseBodyReader(IApplicationBuilder app)
	{
		const int maxBodyLength = 4 * 1024; // 4 KB limit, adjust as needed
		const string requestBodyItemKey = "RequestBody";

		// Buffer and capture request body (async) before Serilog reads context
		app.Use(async (context, next) =>
		{
			var request = context.Request;

			try
			{
				if (request.ContentLength is > 0)
				{
					if (request.ContentLength > maxBodyLength)
					{
						context.Items[requestBodyItemKey] = $"[TooLarge:{request.ContentLength}]";
					}
					else
					{
						request.EnableBuffering();

						using var reader = new StreamReader(request.Body, Encoding.UTF8,
							detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
						var body = await reader.ReadToEndAsync(context.RequestAborted).ConfigureAwait(false);

						// rewind for downstream middleware/controllers
						request.Body.Position = 0;

						context.Items[requestBodyItemKey] = body;
					}
				}
			}
			catch (Exception ex)
			{
				// Fail silently for logging purposes — don't break the request pipeline
				context.Items[requestBodyItemKey] = $"[ReadFailed:{ex.Message}]";
			}

			await next().ConfigureAwait(false);
		});
	}
}

using MentorSync.Notifications.Infrastructure.Hubs;
using MentorSync.SharedKernel;
using Serilog;

namespace MentorSync.API.Extensions;

public static class AppBuilderExtensions
{
	public static void UseAuth(this IApplicationBuilder app)
	{
		app.UseAuthentication();
		app.UseAuthorization();
	}

	public static void UseCustomSwagger(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

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

	public static void UseCustomCorsPolicy(this IApplicationBuilder app)
	{
		app.UseCors(CorsPolicyNames.All);
	}

	public static void MapHubs(this WebApplication app)
	{
		app.MapHub<NotificationHub>("/notificationHub");
	}
}

using System.Reflection;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MentorSync.SharedKernel.Extensions;

/// <summary>
/// Extension methods for endpoint registration and mapping
/// </summary>
public static class EndpointExtensions
{
	/// <summary>
	/// Registers all endpoint implementations from the specified assembly
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	/// <param name="assembly">The assembly to scan for endpoint implementations</param>
	/// <returns>The service collection for method chaining</returns>
	public static void AddEndpoints(this IServiceCollection services, Assembly assembly)
	{
		if (assembly == null)
		{
			return;
		}

		var serviceDescriptors = assembly
			.DefinedTypes
			.Where(type => type is { IsAbstract: false, IsInterface: false } &&
			               type.IsAssignableTo(typeof(IEndpoint)))
			.Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
			.ToArray();

		services.TryAddEnumerable(serviceDescriptors);
	}

	/// <summary>
	/// Maps all registered endpoints to the application route builder
	/// </summary>
	/// <param name="app">The web application to configure</param>
	/// <param name="routeGroupBuilder">Optional route group builder for organizing endpoints</param>
	/// <returns>The application builder for method chaining</returns>
	public static void MapEndpoints(this WebApplication app, RouteGroupBuilder routeGroupBuilder = null)
	{
		if (app == null)
		{
			return;
		}

		var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

		IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

		foreach (var endpoint in endpoints)
		{
			endpoint.MapEndpoint(builder);
		}
	}

	/// <summary>
	/// Marks this endpoint as requiring an antiforgery token.
	/// </summary>
	public static TBuilder RequireAntiforgeryToken<TBuilder>(this TBuilder builder)
		where TBuilder : IEndpointConventionBuilder
	{
		// This attribute implements IAntiforgeryMetadata with RequiresValidation = true
		builder.WithMetadata(new RequireAntiforgeryTokenAttribute());
		return builder;
	}
}

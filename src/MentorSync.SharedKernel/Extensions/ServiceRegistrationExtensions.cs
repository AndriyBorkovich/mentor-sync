using System.Reflection;
using FluentValidation;
using MentorSync.SharedKernel.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.SharedKernel.Extensions;

/// <summary>
/// Extension methods for registering services in the dependency injection container
/// </summary>
public static class ServiceRegistrationExtensions
{
	/// <summary>
	/// Registers command and query handlers from the specified assembly
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	/// <param name="assembly">The assembly to scan for handlers</param>
	public static void AddHandlers(this IServiceCollection services, Assembly assembly)
	{
		services.Scan(scan => scan.FromAssemblies(assembly)
						.AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
							.AsImplementedInterfaces()
							.WithScopedLifetime()
						.AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
							.AsImplementedInterfaces()
							.WithScopedLifetime()
						.AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
							.AsImplementedInterfaces()
							.WithScopedLifetime()
						.AddClasses(c => c.AssignableTo(typeof(INotificationHandler<>)), publicOnly: false)
							.AsImplementedInterfaces()
							.WithScopedLifetime());
	}

	/// <summary>
	/// Registers FluentValidation validators from the specified assembly
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	/// <param name="assembly">The assembly to scan for validators</param>
	public static void AddValidators(this IServiceCollection services, Assembly assembly)
	{
		services.AddValidatorsFromAssembly(assembly);
	}

	/// <summary>
	/// Registers MediatR services with validation pipeline behavior
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	public static void AddMediator(this IServiceCollection services)
	{
		services.AddScoped<IMediator, Mediator>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
	}
}

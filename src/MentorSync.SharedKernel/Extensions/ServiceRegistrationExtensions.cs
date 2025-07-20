using System.Reflection;
using FluentValidation;
using MentorSync.SharedKernel.Abstractions.Messaging;
using MentorSync.SharedKernel.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.SharedKernel.Extensions;
public static class ServiceRegistrationExtensions
{
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
                            .WithScopedLifetime());
    }

    public static void AddValidators(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}

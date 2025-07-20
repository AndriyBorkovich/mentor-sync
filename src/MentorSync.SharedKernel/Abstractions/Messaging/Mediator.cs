using Ardalis.Result;
using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.SharedKernel.Abstractions.Messaging;

public sealed class Mediator(IServiceProvider provider) : IMediator
{
    public async Task<Result<TResult>> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    {
        var handler = provider.GetService<ICommandHandler<TCommand, TResult>>()
            ?? throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");

        var behaviors = provider.GetServices<IPipelineBehavior<TCommand, TResult>>().Reverse();

        Func<Task<Result<TResult>>> handlerDelegate = () => handler.Handle(command, cancellationToken);
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle(command, next, cancellationToken);
        }

        return await handlerDelegate();
    }

    public async Task<Result<TResult>> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
    {
        var handler = provider.GetService<IQueryHandler<TQuery, TResult>>()
            ?? throw new InvalidOperationException($"No handler registered for {typeof(TQuery).Name}");

        var behaviors = provider.GetServices<IPipelineBehavior<TQuery, TResult>>().Reverse();

        Func<Task<Result<TResult>>> handlerDelegate = () => handler.Handle(query, cancellationToken);
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle(query, next, cancellationToken);
        }

        return await handlerDelegate();
    }
}

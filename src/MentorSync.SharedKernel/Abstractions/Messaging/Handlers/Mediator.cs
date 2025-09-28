using Ardalis.Result;
using Microsoft.Extensions.DependencyInjection;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers;

/// <summary>
/// Implementation of the mediator pattern for handling commands and queries
/// </summary>
/// <param name="provider">Service provider for resolving handlers and behaviors</param>
public sealed class Mediator(IServiceProvider provider) : IMediator
{
	/// <summary>
	/// Sends a command through the pipeline with behaviors and returns the result
	/// </summary>
	/// <typeparam name="TCommand">The type of command to send</typeparam>
	/// <typeparam name="TResult">The type of result returned</typeparam>
	/// <param name="command">The command to process</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of the command processing</returns>
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

	/// <summary>
	/// Sends a query through the pipeline with behaviors and returns the result
	/// </summary>
	/// <typeparam name="TQuery">The type of query to send</typeparam>
	/// <typeparam name="TResult">The type of result returned</typeparam>
	/// <param name="query">The query to process</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of the query processing</returns>
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

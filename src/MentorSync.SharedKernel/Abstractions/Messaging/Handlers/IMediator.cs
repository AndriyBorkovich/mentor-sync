using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers;

/// <summary>
/// Interface for mediating between application layers using the mediator pattern
/// </summary>
public interface IMediator
{
	/// <summary>
	/// Sends a command for processing and returns a result
	/// </summary>
	/// <typeparam name="TCommand">The type of command to send</typeparam>
	/// <typeparam name="TResult">The type of result returned</typeparam>
	/// <param name="command">The command to process</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of the command processing</returns>
	Task<Result<TResult>> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
		where TCommand : ICommand<TResult>;

	/// <summary>
	/// Sends a query for processing and returns a result
	/// </summary>
	/// <typeparam name="TQuery">The type of query to send</typeparam>
	/// <typeparam name="TResult">The type of result returned</typeparam>
	/// <param name="query">The query to process</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of the query processing</returns>
	Task<Result<TResult>> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
		where TQuery : IQuery<TResult>;
}

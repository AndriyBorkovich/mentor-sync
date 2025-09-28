using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers.Query;

/// <summary>
/// Interface for handling queries of a specified type and producing a response
/// </summary>
/// <typeparam name="TQuery">The type of query to handle</typeparam>
/// <typeparam name="TResponse">The type of response produced by handling the query</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
	where TQuery : IQuery<TResponse>
{
	/// <summary>
	/// Handles the specified query and returns a result
	/// </summary>
	/// <param name="query">The query to handle</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of handling the query</returns>
	Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

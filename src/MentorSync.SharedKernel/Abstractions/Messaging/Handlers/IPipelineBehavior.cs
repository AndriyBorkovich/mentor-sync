using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers;

/// <summary>
/// Interface for pipeline behaviors that can intercept and process commands/queries
/// </summary>
/// <typeparam name="TInput">The type of input being processed</typeparam>
/// <typeparam name="TOutput">The type of output being returned</typeparam>
public interface IPipelineBehavior<in TInput, TOutput>
{
	/// <summary>
	/// Handles the input by potentially modifying it or the processing pipeline
	/// </summary>
	/// <param name="input">The input to process</param>
	/// <param name="next">The next handler in the pipeline</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of processing</returns>
	Task<Result<TOutput>> Handle(TInput input, Func<Task<Result<TOutput>>> next, CancellationToken cancellationToken = default);
}

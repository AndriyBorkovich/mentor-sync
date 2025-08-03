using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging;

public interface IPipelineBehavior<in TInput, TOutput>
{
	Task<Result<TOutput>> Handle(TInput input, Func<Task<Result<TOutput>>> next, CancellationToken cancellationToken = default);
}

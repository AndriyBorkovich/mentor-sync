using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers.Query;

public interface IQueryHandler<in TQuery, TResponse>
	where TQuery : IQuery<TResponse>
{
	Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

using Ardalis.Result;

namespace MentorSync.SharedKernel.Abstractions.Messaging;

/// <summary>
/// Defines a contract for handling commands of a specified type and producing a response.
/// </summary>
/// <remarks>Implementations of this interface are responsible for processing commands of type <typeparamref
/// name="TCommand"/>  and returning a result encapsulating a response of type <typeparamref
/// name="TResponse"/>.</remarks>
/// <typeparam name="TCommand">The type of the command to handle. Must implement <see cref="ICommand{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response produced by handling the command.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
	where TCommand : ICommand<TResponse>
{
	Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a contract for handling commands of a specified type.
/// </summary>
/// <remarks>Implementations of this interface are responsible for processing commands of type <typeparamref
/// name="TCommand"/>  and returning a <see cref="Result"/> indicating the outcome of the operation. This interface
/// supports asynchronous  execution and allows cancellation through a <see cref="CancellationToken"/>.</remarks>
/// <typeparam name="TCommand">The type of command to be handled. Must implement the <see cref="ICommand"/> interface.</typeparam>
public interface ICommandHandler<in TCommand>
	where TCommand : ICommand
{
	Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

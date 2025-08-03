namespace MentorSync.SharedKernel.Abstractions.Messaging;

/// <summary>
/// Represents a command that produces a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response produced by the command. This type must be non-nullable.</typeparam>
public interface ICommand<TResponse> : IBaseCommand
	where TResponse : notnull;

/// <summary>
/// Represents a command that can be executed.
/// </summary>
/// <remarks>This interface is typically used to define actions or operations that can be triggered,  often in the
/// context of user interfaces, command patterns, or event-driven programming. Implementations of this interface should
/// define the specific behavior of the command.</remarks>
public interface ICommand : IBaseCommand;

/// <summary>
/// Represents a command that can be executed within the application.
/// </summary>
public interface IBaseCommand;

namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers.Query;

/// <summary>
/// A marker interface for queries in the CQRS pattern.
/// Implementations should define the query parameters and response types as needed.
/// </summary>
/// <typeparam name="TResponse">Type of response</typeparam>
public interface IQuery<TResponse> where TResponse : notnull;

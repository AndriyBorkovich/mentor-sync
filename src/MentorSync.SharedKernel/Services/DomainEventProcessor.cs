using MentorSync.SharedKernel.Abstractions.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.SharedKernel.Services;

/// <summary>
/// Background service that processes domain events from a channel-based publisher
/// </summary>
/// <param name="pubSub">The channel publisher-subscriber for domain events</param>
/// <param name="sp">Service provider for resolving event handlers</param>
/// <param name="logger">Logger for event processing activities</param>
public sealed class DomainEventProcessor(
	ChannelPubSub<DomainEvent> pubSub,
	IServiceProvider sp,
	ILogger<DomainEventProcessor> logger) : BackgroundService
{
	/// <summary>
	/// Executes the background service, continuously processing domain events
	/// </summary>
	/// <param name="stoppingToken">Token to signal when the service should stop</param>
	/// <returns>A task representing the asynchronous operation</returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				logger.LogInformation("Processing of events started");
				await ProcessAsync(stoppingToken);
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error in domain event processor loop");
			}
			finally
			{
				logger.LogInformation("Processing of events finished. Waiting for 5 sec..");
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
		}
	}

	private async Task ProcessAsync(CancellationToken stoppingToken)
	{
		await foreach (var domainEvent in pubSub.Reader.ReadAllAsync(stoppingToken))
		{
			var eventType = domainEvent.GetType();
			logger.LogInformation("Processing domain event: {EventType}", eventType);

			var handlerInterface = typeof(INotificationHandler<>).MakeGenericType(eventType);
			foreach (var handler in sp.GetServices(handlerInterface))
			{
				try
				{
					// dynamic dispatch to HandleAsync(T, CancellationToken)
					dynamic dynHandler = handler;
					if (dynHandler is not null)
					{
						await dynHandler.HandleAsync((dynamic)domainEvent, stoppingToken);
					}
				}
				catch (Exception ex)
				{
					if (handler is not null)
					{
						logger.LogError(ex,
							"Error in handler {Handler} for event {Event}",
							handler.GetType().Name, eventType.Name);
					}
				}
			}
		}
	}
}

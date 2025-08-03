using MentorSync.SharedKernel.Abstractions.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.SharedKernel.Services;

public sealed class DomainEventProcessor(
	ChannelPubSub<DomainEvent> pubSub,
	IServiceProvider sp,
	ILogger<DomainEventProcessor> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken ct)
	{
		while (!ct.IsCancellationRequested)
		{
			try
			{
				logger.LogInformation("Processing of events started");
				await foreach (var domainEvent in pubSub.Reader.ReadAllAsync(ct))
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
							await dynHandler.HandleAsync((dynamic)domainEvent, ct);
						}
						catch (Exception ex)
						{
							logger.LogError(ex,
								"Error in handler {Handler} for event {Event}",
								handler.GetType().Name, eventType.Name);
						}
					}
				}
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
				await Task.Delay(TimeSpan.FromSeconds(5), ct);
			}
		}
	}
}

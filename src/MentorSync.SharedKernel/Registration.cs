using MentorSync.SharedKernel.Abstractions.DomainEvents;
using MentorSync.SharedKernel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.SharedKernel;

/// <summary>
/// Provides registration methods for shared services used across the application
/// </summary>
public static class Registration
{
	/// <summary>
	/// Registers all shared services including Azure Blob Storage, MediatR, and domain event processing
	/// </summary>
	/// <param name="builder">The host application builder to configure</param>
	/// <example>
	/// <code>
	/// var builder = WebApplication.CreateBuilder(args);
	/// builder.AddSharedServices();
	/// </code>
	/// </example>
	public static void AddSharedServices(this IHostApplicationBuilder builder)
	{
		if (builder == null)
		{
			return;
		}

		builder.AddAzureBlobServiceClient("files-blobs");

		builder.Services.AddMediator();

		builder.Services.AddSingleton(typeof(ChannelPubSub<>));
		builder.Services.AddSingleton(typeof(IPublisher<>), typeof(ChannelPubSub<>));

		builder.Services.AddSingleton<IDomainEventsDispatcher, ChannelDomainEventsDispatcher>();
		builder.Services.AddHostedService<DomainEventProcessor>();
	}
}

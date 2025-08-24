using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.SharedKernel;

public static class Registration
{
	public static void AddSharedServices(this IHostApplicationBuilder builder)
	{
		builder.AddAzureBlobServiceClient("files-blobs");

		builder.Services.AddMediator();

		builder.Services.AddSingleton(typeof(ChannelPubSub<>));
		builder.Services.AddSingleton(typeof(IPublisher<>), typeof(ChannelPubSub<>));

		builder.Services.AddSingleton<IDomainEventsDispatcher, ChannelDomainEventsDispatcher>();

		builder.Services.AddHostedService<DomainEventProcessor>();
	}
}

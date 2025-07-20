using MentorSync.SharedKernel.Abstractions.Messaging;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.SharedKernel;

public static class Registration
{
    public static void AddSharedServices(this IHostApplicationBuilder builder)
    {
        builder.AddAzureBlobClient("files-blobs");

        builder.Services.AddMediator();

        builder.Services.AddSingleton<IDomainEventsDispatcher, MediatorDomainEventsDispatcher>();
    }
}

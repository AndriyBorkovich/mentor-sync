﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Scheduling.Features.Booking.UpdatePending;

/// <summary>
/// Update all pending bookings to be in  "no show" status
/// </summary>
/// <param name="logger">Logger</param>
/// <param name="serviceProvider">Service provider</param>
public sealed class UpdatePendingBookingsJob(
    ILogger<UpdatePendingBookingsJob> logger,
    IServiceProvider serviceProvider)
        : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                var updatedCount =  await sender.Send(new UpdatePendingBookingsCommand(), stoppingToken);
                if (updatedCount.IsSuccess && updatedCount.Value > 0)
                {
                    logger.LogInformation("{Count} pending bookings updated successfully.", updatedCount.Value);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating pending bookings");
            }

            await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
        }
    }
}

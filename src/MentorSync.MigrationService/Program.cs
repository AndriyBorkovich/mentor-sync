using MentorSync.MigrationService;
using MentorSync.Notifications;
using MentorSync.ServiceDefaults;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Services;
using MentorSync.Users.Data;
using MentorSync.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSingleton<IDomainEventsDispatcher, MediatorDomainEventsDispatcher>();
builder.AddNotificationsModule();
builder.AddNpgsqlDbContext<UsersDbContext>(
    connectionName: GeneralConstants.DatabaseName,
    configureSettings: c => c.DisableTracing = true,
    configureDbContextOptions: opt =>
    {
        opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
    });

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();
host.Run();

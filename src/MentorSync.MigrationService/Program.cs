using MentorSync.MigrationService;
using MentorSync.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMockEventDispatcher();

builder.AddDbContexts();

builder.Services.AddMigrationsWorker();

var host = builder.Build();

await host.RunAsync();

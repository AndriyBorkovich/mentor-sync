using MentorSync.MigrationService;
using MentorSync.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMockEventDispatcher();

builder.AddDbContexts();

builder.Services.AddMigrationWorker();

var host = builder.Build();
host.Run();

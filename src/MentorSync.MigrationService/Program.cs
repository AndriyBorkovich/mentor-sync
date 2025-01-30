using MentorSync.MigrationService;
using MentorSync.ServiceDefaults;
using MentorSync.SharedKernel;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<UsersDbContext>(
    connectionName: GeneralConstants.DatabaseName,
    configureDbContextOptions: opt =>
    {
        opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Users));
    });

var host = builder.Build();
host.Run();
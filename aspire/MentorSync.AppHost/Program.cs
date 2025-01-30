var builder = DistributedApplication.CreateBuilder(args);

// postgres database
var postgres = builder.AddPostgres("postgres-db")
                        .WithPgAdmin(containerBuilder =>
                            containerBuilder.WithLifetime(ContainerLifetime.Persistent))
                        .WithDataVolume(name: "postgres-data", isReadOnly: false)
                        .WithLifetime(ContainerLifetime.Persistent);
var postgresDb = postgres.AddDatabase("MentorSyncDb");

// API project
builder.AddProject<Projects.MentorSync_API>("api")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

// migrations service
builder.AddProject<Projects.MentorSync_MigrationService>("migration-service")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
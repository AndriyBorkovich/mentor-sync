var builder = DistributedApplication.CreateBuilder(args);

// Postgres database
var postgres = builder.AddPostgres("postgres-db")
                        .WithPgAdmin(containerBuilder =>
                            containerBuilder.WithLifetime(ContainerLifetime.Persistent))
                        .WithDataVolume(name: "postgres-data", isReadOnly: false)
                        .WithLifetime(ContainerLifetime.Persistent);
var postgresDb = postgres.AddDatabase("MentorSyncDb");

// API project
builder.AddProject<Projects.MentorSync_API>("MentorSyncApi")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
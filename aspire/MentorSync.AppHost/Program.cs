var builder = DistributedApplication.CreateBuilder(args);

// Postgres database
var postgres = builder.AddPostgres("postgres-db")
                        .WithPgAdmin()
                        .WithDataVolume(name: "postgres-data", isReadOnly: false);
var postgresDb = postgres.AddDatabase("MentorSyncDb");

// API project
builder.AddProject<Projects.MentorSync_API>("MentorSyncApi")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
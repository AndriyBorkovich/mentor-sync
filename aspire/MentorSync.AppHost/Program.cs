var builder = DistributedApplication.CreateBuilder(args);

// postgres database
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var postgres = builder.AddAzurePostgresFlexibleServer("postgres-db")
                    .WithPasswordAuthentication(username, password);
                       // for local development
                    //    .RunAsContainer(b => 
                    //     {
                    //         b.WithPgAdmin(cb => cb.WithLifetime(ContainerLifetime.Persistent));
                    //         b.WithDataVolume(name: "postgres-data", isReadOnly: false);
                    //         b.WithLifetime(ContainerLifetime.Persistent);
                    //     });
var postgresDb = postgres.AddDatabase("MentorSyncDb");

// API project
builder.AddProject<Projects.MentorSync_API>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

// migrations service
builder.AddProject<Projects.MentorSync_MigrationService>("migration-service")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
var builder = DistributedApplication.CreateBuilder(args);

// postgres database
var usernameDb = builder.AddParameter("username", secret: true);
var passwordDb = builder.AddParameter("password", secret: true);

var postgres = builder.AddAzurePostgresFlexibleServer("postgres-db")
                      .WithPasswordAuthentication(usernameDb, passwordDb);

if (!builder.ExecutionContext.IsPublishMode)
{
    postgres = postgres.RunAsContainer(b =>
    {
        b.WithPgAdmin(cb => cb.WithLifetime(ContainerLifetime.Persistent));
        b.WithDataVolume(name: "postgres-data", isReadOnly: false);
        b.WithLifetime(ContainerLifetime.Persistent);
    });
}

var postgresDb = postgres.AddDatabase("MentorSyncDb");

// elasticsearch and kibana
var passwordElastic = builder.AddParameter("password-es", secret: true);
var elasticsearch = builder.AddElasticsearch(name: "elasticsearch", password: passwordElastic)
    .WithDataVolume("elasticsearch-data")
    .WithEnvironment("discovery.type", "single-node")
    .WithEnvironment("xpack.security.enabled", "false")
    .WithEndpoint(port: 9200, targetPort: 9200)
    .WithLifetime(ContainerLifetime.Persistent);

var kibana = builder
    .AddContainer(name: "kibana-client", image: "kibana", tag: "8.15.3")
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch)
    .WithEndpoint(port: 5601, targetPort: 5601)
    .WithLifetime(ContainerLifetime.Persistent);

// API project
builder.AddProject<Projects.MentorSync_API>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

// migrations service
builder.AddProject<Projects.MentorSync_MigrationService>("migration-service")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
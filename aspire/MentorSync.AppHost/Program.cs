using Aspire.Hosting.Azure;

var builder = DistributedApplication.CreateBuilder(args);

// Azure postgres database
var usernameDb = builder.AddParameter("postgre-username", secret: true);
var passwordDb = builder.AddParameter("postgre-password", secret: true);

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

// mongo database
var userNameMongo = builder.AddParameter("mongo-username", secret: true);
var mongoPassword = builder.AddParameter("mongo-password", secret: true);

var mongo = builder.AddMongoDB("mongo", userName: userNameMongo, password: mongoPassword)
                   .WithDataVolume("mongo-data")
                   .WithLifetime(ContainerLifetime.Persistent);
var mongodb = mongo.AddDatabase("mongodb");

// elasticsearch and kibana
var passwordElastic = builder.AddParameter("elastic-password", secret: true);
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

// Azure SMTP server
var communicationService = builder.AddBicepTemplate(name: "communication-service", "../bicep-templates/communication-service.module.bicep")
                                  .WithParameter("isProd", false)
                                  .WithParameter("communicationServiceName", "cs-mentorsync-dev")
                                  .WithParameter("emailServiceName", "es-mentorsync-dev")
                                  .WithParameter(AzureBicepResource.KnownParameters.KeyVaultName);

var smtpConnectionString = communicationService.GetSecretOutput("cs-connectionString");

// migrations service
builder.AddProject<Projects.MentorSync_MigrationService>("migration-service")
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithReference(mongodb)
    .WaitFor(mongodb);

// API project
builder.AddProject<Projects.MentorSync_API>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithEnvironment("ConnectionStrings__EmailService", smtpConnectionString);

builder.Build().Run();

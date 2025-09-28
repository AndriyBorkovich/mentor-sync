using System.Globalization;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// Azure blob storage
var azureStorage = builder.AddAzureStorage("mentor-sync-storage");

if (!builder.ExecutionContext.IsPublishMode)
{
	azureStorage.RunAsEmulator(azurite =>
		azurite.WithLifetime(ContainerLifetime.Persistent)
			   .WithDataVolume(name: "emulator-local-storage"));
}

var blobs = azureStorage.AddBlobs("files-blobs");

// Azure postgres database
var usernamePostgres = builder.AddParameter("postgre-username", secret: true);
var passwordPostgres = builder.AddParameter("postgre-password", secret: true);

var postgres = builder.AddAzurePostgresFlexibleServer("postgres-db")
					  .WithPasswordAuthentication(usernamePostgres, passwordPostgres);

if (!builder.ExecutionContext.IsPublishMode)
{
	postgres = postgres.RunAsContainer(b =>
	{
		b.WithPgAdmin(cb =>
			cb.WithImageTag("latest").WithLifetime(ContainerLifetime.Persistent));
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
				   .WithLifetime(ContainerLifetime.Persistent)
				   .WithMongoExpress(cb =>
						cb.WithImageTag("latest").WithLifetime(ContainerLifetime.Persistent));
var mongodb = mongo.AddDatabase("mongodb");

// Azure SMTP server
var csKeyVault = builder.AddParameter("cs-keyvault", secret: true);
/*var communicationService = */
builder.AddBicepTemplate(name: "communication-service", "../bicep-templates/communication-service.module.bicep")
								  .WithParameter(name: "isProd", value: false)
								  .WithParameter("communicationServiceName", "cs-mentorsync-dev")
								  .WithParameter("emailServiceName", "es-mentorsync-dev")
								  .WithParameter("keyVaultName", csKeyVault);

var smtpConnectionString = builder.Configuration.GetValue<string>("cs-connectionString");

// migrations service
builder.AddProject<Projects.MentorSync_MigrationService>("migration-service")
	.WithReference(postgresDb)
	.WaitFor(postgresDb);

var uiPort = builder.Configuration.GetValue<int>("UI_PORT");

// API project
var api = builder.AddProject<Projects.MentorSync_API>("api")
	.WithExternalHttpEndpoints()
	.WithReference(blobs)
	.WaitFor(blobs)
	.WithReference(postgresDb)
	.WaitFor(postgresDb)
	.WithReference(mongodb)
	.WaitFor(mongodb)
	.WithEnvironment("ConnectionStrings__EmailService", smtpConnectionString)
	.WithEnvironment(name: "UI_PORT", value: uiPort.ToString(CultureInfo.InvariantCulture));

// React + vite UI project
builder.AddNpmApp(name: "ui", workingDirectory: "../../src/MentorSync.UI", scriptName: "dev")
	.WithReference(api)
	.WaitFor(api)
	.WithEnvironment("BROWSER", "none")
	.WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
	.WithHttpEndpoint(port: uiPort, env: "VITE_PORT")
	.WithExternalHttpEndpoints()
	.WithNpmPackageInstallation()
	.PublishAsDockerFile();

await builder.Build().RunAsync().ConfigureAwait(false);

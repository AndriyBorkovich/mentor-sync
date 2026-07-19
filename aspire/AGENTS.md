# Aspire and infrastructure guidance

This subtree owns local orchestration, service wiring, Azure Developer CLI configuration, and Bicep templates.

## Orchestration

- Treat `MentorSync.AppHost/Program.cs` as the source of truth for resource names and dependencies: PostgreSQL, MongoDB, blob storage, migration service, API, UI, and communication-service parameters.
- Keep local and publish behavior explicit through `builder.ExecutionContext.IsPublishMode`; do not make cloud resources silently replace local emulators/containers.
- Preserve dependency ordering: migrations wait for PostgreSQL; the API waits for its stores; the UI waits for the API.
- Pass endpoints and secrets through Aspire references, parameters, configuration, or environment variables. Never hardcode credentials or generated connection strings.
- Changes to resource names are compatibility changes: update all references, environment variables, manifests/templates, documentation, and deployment configuration together.

## Bicep and generated artifacts

- Keep Bicep modules idempotent, parameterized, and consistent with the root `main.bicep` and `main.parameters.json` composition.
- Prefer symbolic references and outputs over reconstructing Azure resource IDs or endpoints as strings.
- Do not edit generated Aspire manifest/template output unless the task explicitly targets generated deployment artifacts; change the AppHost/source template and regenerate instead.
- Treat production data stores, role assignments, and resource deletions as destructive. Do not deploy, purge, rotate secrets, or change live resources without explicit authorization.

## Validation

Use checks appropriate to the touched files:

```powershell
dotnet build aspire/MentorSync.AppHost/MentorSync.AppHost.csproj
dotnet build aspire/MentorSync.ServiceDefaults/MentorSync.ServiceDefaults.csproj
az bicep build --file aspire/MentorSync.AppHost/infra/main.bicep
```

Validate YAML changes with the repository `.yamllint` configuration. Running the AppHost or Azure deployment may require containers, ports, credentials, and an Azure login; report those prerequisites rather than weakening configuration.

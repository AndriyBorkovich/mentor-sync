# .NET 10 and Aspire 13 migration

This guide records the decisions and compatibility review for the MentorSync
migration from .NET 9 and Aspire 9.5.2 to .NET 10 and Aspire 13.4.6.

## Version policy

- The repository SDK is pinned to `10.0.110` in `global.json`, with
  `latestPatch` roll-forward.
- All .NET projects target `net10.0` through `Directory.Build.props`.
- Microsoft framework packages use the latest .NET 10 servicing patch.
- Aspire packages and the AppHost SDK use one aligned version: `13.4.6`.
- EF Core tooling is repository-local and pinned to `10.0.10`.
- PostgreSQL development containers remain on `17.6`. Aspire 13.4 defaults to
  PostgreSQL 18.3, whose data directory layout is not compatible with an
  existing PostgreSQL 17 persistent volume.

## Applied breaking changes

### .NET and ASP.NET Core

- Framework, SDK, CI, ASP.NET Core, EF Core, and Microsoft.Extensions package
  versions were moved to .NET 10 together.
- Redundant framework package references such as `System.Text.Json` and
  `Microsoft.AspNetCore.OpenApi` were removed.
- Swashbuckle 10 uses Microsoft.OpenApi 2. Its types moved from
  `Microsoft.OpenApi.Models` to `Microsoft.OpenApi`, security scheme references
  use `OpenApiSecuritySchemeReference`, and global security requirements are
  created from the generated `OpenApiDocument`.
- .NET 10 changes `BackgroundService.ExecuteAsync` so its entire body runs on a
  background task. The migration worker already uses asynchronous work and does
  not rely on synchronous startup blocking.
- .NET 10 preserves null configuration values and uses W3C trace propagation by
  default. No MentorSync configuration or custom trace propagation code relies
  on the previous behavior.

Reference:

- [.NET 10 breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/10)
- [ASP.NET Core 9 to 10 migration](https://learn.microsoft.com/aspnet/core/migration/90-to-100)
- [Swashbuckle 10 migration](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/migrating-to-v10.md)

### Entity Framework Core and Npgsql

- EF Core, its design-time tools, the Npgsql provider, and Aspire's Npgsql
  integration were upgraded as a unit.
- Existing migrations and snapshots are compiled into their owning module
  assemblies. They are marked as generated analyzer inputs; their generated
  source was not modified.
- EF Core 10 translates parameterized collections with scalar parameters by
  default and simplifies generated SQL parameter names. Code must not parse or
  snapshot exact generated parameter names.
- Npgsql EF Core 10 translates PostgreSQL array `Contains` to `= ANY(...)` by
  default. A modeled GIN index retains the containment translation when it is
  important for query performance.
- Npgsql 10 maps untyped PostgreSQL `date`, `time`, and `cidr` reads to
  `DateOnly`, `TimeOnly`, and `IPNetwork`. MentorSync uses typed EF mappings and
  does not use the affected untyped ADO.NET APIs.
- Npgsql tracing and metric names follow current OpenTelemetry semantic
  conventions. External dashboards or alerts that use old Npgsql metric names
  must be updated separately.
- Npgsql 10 only uses a supplied root CA for TLS validation rather than
  combining it with the system CA store. Production certificate chains should
  be reviewed before deployment.

Validation confirmed that all five PostgreSQL contexts match their existing
snapshots and that EF Core 10 can generate an idempotent SQL script for every
context. No migration was created or applied, and no database was modified.

Reference:

- [EF Core 10 breaking changes](https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/breaking-changes)
- [Npgsql EF Core 10 release notes](https://www.npgsql.org/efcore/release-notes/10.0.html)
- [Npgsql 10 release notes](https://www.npgsql.org/doc/release-notes/10.0.html)

### Aspire

- The AppHost uses the Aspire 13 SDK project format. The SDK supplies
  `Aspire.Hosting.AppHost`, so the explicit package reference was removed.
- `Aspire.Hosting.NodeJs` was renamed to `Aspire.Hosting.JavaScript`.
- The community `AddNpmApp` integration was replaced by Aspire's first-party
  `AddViteApp` integration and `PublishAsStaticWebsite`.
- Vite reads the injected `PORT` variable. Service discovery continues to use
  scheme-based `services__api__https__*` and `services__api__http__*` variables,
  which is required by Aspire 13.2.
- The MongoDB client integration package was renamed from
  `Aspire.MongoDB.Driver.v3` to `Aspire.MongoDB.Driver`.
- The legacy Aspire workload install was removed from CI; Aspire 9 and later
  use the SDK and packages instead of a workload.
- Aspire 13 deployment pipelines replace the removed publishing callback APIs.
  MentorSync does not use those APIs.
- Azure Container Apps and App Service publishing now constrain
  `DefaultAzureCredential` to managed identity. MentorSync's Azure deployment
  must use managed identity unless it explicitly overrides that behavior.
- Aspire 13.4 changes its default PostgreSQL image from 17.6 to 18.3. MentorSync
  pins 17.6 to preserve the existing local persistent-volume format.

Reference:

- [Upgrade Aspire](https://aspire.dev/whats-new/upgrade-aspire/)
- [Aspire 13 breaking changes](https://aspire.dev/whats-new/aspire-13/#breaking-changes)
- [Aspire 13.2 breaking changes](https://aspire.dev/whats-new/aspire-13-2/#breaking-changes)
- [Aspire 13.4 breaking changes](https://aspire.dev/whats-new/aspire-13-4/#breaking-changes)

## Upgrade validation

Run these commands from the repository root:

```powershell
dotnet tool restore
dotnet restore --locked-mode
dotnet build --no-restore
dotnet test --no-build --verbosity normal
dotnet package list --vulnerable --include-transitive --no-restore
```

For every PostgreSQL context, also run:

```powershell
dotnet ef migrations has-pending-model-changes `
  --context <DbContext> `
  --project <module-project> `
  --startup-project src/MentorSync.API/MentorSync.API.csproj `
  --no-build
```

Generate and review idempotent migration scripts before any production
deployment. Do not apply migrations automatically as part of a package upgrade.

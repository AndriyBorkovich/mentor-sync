# Migration service guidance

- This project targets .NET 10 and EF Core 10. Keep EF Core, its design-time tooling, Npgsql, and Aspire's Npgsql integration on compatible major versions.
- This service applies every relational module's migrations and development seeders, then exits. Keep execution deterministic and safe to retry.
- Add a DbContext to the migration sequence when a new relational module is introduced, and preserve dependency-aware ordering for seeders.
- Keep migration and seeding APIs asynchronous and propagate the host cancellation token.
- Seeders must be idempotent and must not overwrite user-created data. Never enable `CleanDatabase`/`EnsureDeleted` in a normal run.
- Create EF migrations in the owning module project, not in this service. Review generated migration operations before accepting them, especially drops, renames, default values, and data transforms.
- Do not edit existing generated migrations or snapshots during package-only upgrades. Use `dotnet ef migrations has-pending-model-changes` for every context and review an idempotent script before applying database changes.
- Validate with a build and, when a disposable local database is available, run the service twice to confirm migrations and seeds are repeatable.

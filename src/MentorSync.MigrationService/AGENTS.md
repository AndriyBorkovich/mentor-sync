# Migration service guidance

- This service applies every relational module's migrations and development seeders, then exits. Keep execution deterministic and safe to retry.
- Add a DbContext to the migration sequence when a new relational module is introduced, and preserve dependency-aware ordering for seeders.
- Keep migration and seeding APIs asynchronous and propagate the host cancellation token.
- Seeders must be idempotent and must not overwrite user-created data. Never enable `CleanDatabase`/`EnsureDeleted` in a normal run.
- Create EF migrations in the owning module project, not in this service. Review generated migration operations before accepting them, especially drops, renames, default values, and data transforms.
- Validate with a build and, when a disposable local database is available, run the service twice to confirm migrations and seeds are repeatable.

# Backend module guidance

This file applies to every module under `src/Modules`; each domain directory may add narrower rules.

## Boundaries and layout

- A module owns its `Domain`, `Data`, `Features`, `Services`, `Infrastructure`, registration, EF migrations, and PostgreSQL schema.
- Expose cross-module DTOs and service interfaces only through `MentorSync.<Domain>.Contracts`. Contracts projects may depend on the SharedKernel but must not depend on implementation projects.
- An implementation may consume another domain only through that domain's Contracts project. Pass foreign identities as stable IDs; do not navigate or query another module's DbContext.
- Register new services, validators, endpoints, DbContexts, and background jobs in the module's existing `ModuleRegistration` pattern.

## Feature implementation

- Organize new operations as vertical slices under `Features/<Feature>/<Operation>` and follow the closest command/query/handler/endpoint example.
- Use `ICommand` for writes and `IQuery` for reads; handlers return `Ardalis.Result` and use the module DbContext directly.
- Define Minimal API routes in `IEndpoint` implementations, map request/response types explicitly, and apply the appropriate authorization policy. Default to protected access unless anonymous access is an intentional requirement.
- Validate input with FluentValidation and enforce ownership/business invariants in the handler or domain model. Never trust a user ID supplied by the client when the authenticated principal provides it.
- Propagate `CancellationToken` to EF, HTTP, storage, and messaging calls. Use `AsNoTracking` and projection for read queries; use transactions only across changes owned by this module.
- Publish or consume contracts/domain events for cross-module workflows; do not introduce direct implementation references to make orchestration easier.

## Persistence changes

- Configure entities in the owning DbContext and retain the module's default schema.
- Generate migrations in the owning implementation project and keep them in its `Data/Migrations` folder. Do not rewrite old migrations that may already be applied.
- Update the migration service when a newly persistent module/DbContext must participate in startup migration.

## Validation

- Add behavior-focused tests where the repository has an appropriate test home. At minimum, build the affected project and its consumers.
- Always run `dotnet test tests/MentorSync.ArchitectureTests/MentorSync.ArchitectureTests.csproj` after dependency, visibility, namespace, endpoint, handler, or module-layout changes.

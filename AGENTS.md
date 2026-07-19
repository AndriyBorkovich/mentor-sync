# MentorSync agent guidance

## Repository map

- `src/MentorSync.API`: ASP.NET Core host and cross-cutting HTTP middleware.
- `src/MentorSync.MigrationService`: EF Core migrations and development seed orchestration.
- `src/MentorSync.SharedKernel`: shared abstractions and infrastructure used by backend modules.
- `src/Modules`: modular-monolith business modules.
- `src/MentorSync.UI`: React, TypeScript, and Vite frontend.
- `aspire`: .NET Aspire orchestration and Azure/Bicep infrastructure.
- `tests/MentorSync.ArchitectureTests`: executable architecture and naming rules.
- `docs/Guides`: detailed implementation patterns; use the existing code as the final authority when older docs disagree with it.

## Working agreements

- Keep changes narrowly scoped and preserve unrelated user changes.
- Inspect nearby implementations before adding a new pattern or dependency.
- Follow `.editorconfig`; do not hand-edit generated files, lock files, EF migrations, or deployment output unless the task requires it.
- Never commit secrets. Configuration values and credentials belong in environment variables, user secrets, Aspire parameters, or Key Vault references.
- Do not change SDK/framework/dependency versions merely to complete an unrelated task.
- Update relevant documentation when behavior, configuration, public contracts, or developer workflows change.

## Architecture invariants

- This is a .NET 9 modular monolith. Business modules are independently registered and own their data models and schema.
- A module may reference another module only through that module's `MentorSync.<Domain>.Contracts` project. Never reference another module's implementation project, `Data`, `Domain`, `Features`, or `Infrastructure` namespaces.
- Put genuinely shared technical abstractions in `MentorSync.SharedKernel`; do not move domain behavior there to bypass module boundaries.
- Backend HTTP APIs use Minimal APIs through `IEndpoint`; do not introduce MVC controllers.
- Backend application flows use the custom `ICommand`/`IQuery`/`IMediator` abstractions and `Ardalis.Result`, not MediatR or a parallel mediator stack.
- Use EF Core `DbContext` directly in handlers/services. Do not add repository or unit-of-work wrappers.

## C# conventions

Treat `.editorconfig`, `Directory.Build.props`, enabled analyzers, and architecture tests as executable rules. In particular:

### Formatting and declarations

- Use tabs with width 4 for C# and keep a final newline with no trailing whitespace. Do not reformat unrelated code.
- Use file-scoped namespaces that match the folder structure. Place `using` directives outside namespaces, remove unused imports, and sort `System` directives first.
- Use Allman braces for all declarations and control-flow blocks. Braces are required even for single-line bodies; place `else`, `catch`, and `finally` on a new line.
- Use the modifier order configured in `.editorconfig` and declare accessibility on non-interface members.
- Prefer top-level statements for executable entry points and primary constructors/constructor injection where they keep dependencies clear and match nearby code.
- Seal classes that are not designed for inheritance. Prefer `sealed record` for immutable commands, queries, requests, responses, events, and value-like DTOs; use classes for mutable entities and stateful services.

### Naming and type usage

- Use PascalCase for namespaces, types, methods, properties, events, and constants; interfaces start with `I`.
- Use camelCase for parameters and locals, and `_camelCase` for private/internal fields. Make fields `readonly` whenever they are assigned only during construction.
- Follow architecture-tested suffixes: `Command`, `CommandHandler`, `Query`, `QueryHandler`, `Endpoint`, `Validator`, `Request`, `Response`, and `DbContext`.
- Keep request/response DTOs, commands, queries, handlers, validators, and endpoints in the owning `MentorSync.<Module>.Features.<Feature>` namespace.
- Name ordinary asynchronous APIs with an `Async` suffix. Preserve framework/interface names such as `Handle`, `HandleAsync`, `ExecuteAsync`, and `MapEndpoint` exactly as required by their contracts.
- Follow the configured `var` rules rather than applying one blanket style: use `var` for built-in types and when inference improves readability; retain an explicit type when it communicates information not obvious from the initializer.
- Use predefined type keywords (`string`, `int`, `bool`) instead of framework aliases in declarations.

### Expressions and null handling

- Prefer `is null`/`is not null`, property/type patterns, `not` patterns, and pattern matching over casts followed by null checks.
- Prefer switch expressions over switch statements when expressing a value mapping, and use conditional/throw expressions only when they remain easier to read.
- Prefer null propagation, coalescing, object/collection initializers, collection expressions, compound assignment, and simplified interpolation when supported by the target type.
- Guard required arguments at boundaries with helpers such as `ArgumentNullException.ThrowIfNull`; do not add redundant checks where types or framework binding already guarantee a value.
- Do not introduce nullable-reference-type policy changes in isolated files. Match the containing project and model absence explicitly where the existing API permits it.

### Async, data, errors, and logging

- All I/O must be asynchronous. Propagate `CancellationToken` through handlers, EF Core, HTTP, storage, messaging, and background work.
- Never use `async void` except required event handlers, and never block with `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- In reusable library/infrastructure code, follow nearby use of `ConfigureAwait(false)`; it is not required in ASP.NET Core request code.
- Use the owning EF Core `DbContext` directly. For read-only queries, use `AsNoTracking()`, filter before materialization, project only required columns, and paginate unbounded collections.
- Return expected application failures through `Ardalis.Result`; reserve exceptions for unexpected or invariant-breaking conditions handled by the global exception pipeline.
- Use structured `ILogger` templates with named properties, not interpolated strings. Never log credentials, tokens, personal message content, or connection strings.
- Public APIs require useful XML documentation because documentation generation is enabled. Document intent, parameters, results, and meaningful exceptions; avoid comments that merely restate code.

## Validation

Run the smallest relevant checks during iteration, then the broader checks warranted by the change:

```powershell
dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal
dotnet test tests/MentorSync.ArchitectureTests/MentorSync.ArchitectureTests.csproj
```

For UI-only changes, follow `src/MentorSync.UI/AGENTS.md`. For infrastructure changes, follow `aspire/AGENTS.md`. If a check cannot run because required services or credentials are unavailable, state exactly what was and was not verified.

## Git conventions

- Follow `.github/git-commit-instructions.md` and `docs/GettingStarted/semantic-versioning.md`.
- Use Conventional Commits with a lowercase, specific description of at most 50 characters: `<type>: <description>`.
- Use `(MINOR)` only for backward-compatible features and `(MAJOR)` for breaking changes. Omit both flags for patch-level work; use `docs:` for documentation-only changes.
- Stage only files that belong to the requested change. Do not include unrelated working-tree changes in a commit.

## Review guidelines

- Report only actionable, high-confidence findings.
- Prioritize broken module boundaries, authorization gaps, data-loss risks, migration/seed safety, async blocking, contract incompatibility, and missing behavior-focused tests.
- Treat architecture tests as constraints, not as a substitute for reviewing runtime behavior.

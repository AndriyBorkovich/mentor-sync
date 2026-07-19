# Architecture tests guidance

This project contains xUnit v3 and ArchUnitNET tests that make MentorSync's structural rules executable. It is not the home for domain behavior or endpoint integration tests.

## Test intent and structure

- Keep tests grouped by concern: layer visibility/dependencies, CQRS and vertical-slice placement, modular-monolith boundaries, and naming conventions.
- Name tests as declarative rules using the existing `<Subject>Should<Expectation>` style. One test should communicate one architectural invariant.
- Build ArchUnit rules with the fluent `Types`/`Classes`/`Interfaces` predicates and finish them with a specific `Because(...)` explanation before `Check(Architecture)`.
- Reuse `ArchUnitExtensions.Architecture` rather than rebuilding the architecture in each test. Keep architecture loading lazy and centralized.
- When adding a production or Contracts assembly that should participate in the rules, add its marker type/assembly to `ArchUnitExtensions` and add the required project reference to the test project.
- Namespace and name matchers are regex/pattern sensitive. Anchor exact module namespaces where necessary and ensure a rule selects at least the intended production types rather than passing over an empty set.
- Exclude compiler-generated members, accessors, operators, migrations, or explicit exceptional contract types only when they would create a documented false positive. Keep exclusions as narrow as possible.

## Changing architectural rules

- Prefer adding a focused regression rule when a review uncovers a repeatable structural violation.
- Do not weaken or delete a rule merely to make a production change pass. First fix the production structure; change the rule only when the intended architecture itself changed.
- When architecture intentionally changes, update the relevant repository/module `AGENTS.md`, architecture documentation, assembly loading, and tests in the same change.
- Rules should validate observable structure—names, namespaces, visibility, interfaces, and dependencies—not implementation details that make safe refactoring unnecessarily difficult.
- Keep assertion failure output understandable by using precise predicates and `Because` text.

## Conventions

- Follow the root C# and `.editorconfig` rules: file-scoped namespace, Allman braces, tabs, ordered usings, XML documentation for public test fixtures/methods, and no unused parameters.
- Test fixtures are `public sealed class`; tests use `[Fact]` unless multiple meaningful cases require `[Theory]`.
- Reuse shared predicates/helpers for repeated non-trivial selection logic, but keep a rule readable from the test body.
- Do not introduce mocks, databases, network access, time dependence, or mutable global state; architecture tests must remain deterministic and fast.

## Validation

Run the focused project from the repository root:

```powershell
dotnet test tests/MentorSync.ArchitectureTests/MentorSync.ArchitectureTests.csproj
```

When changing loaded assemblies or dependencies, also run `dotnet build` for the full solution to catch reference and analyzer failures.

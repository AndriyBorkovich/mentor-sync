# Architecture Testing in MentorSync

This document describes the automated architecture testing framework that validates MentorSync's modular monolith design and enforces architectural constraints.

## Overview

MentorSync uses **ArchUnitNET** (v0.12.3) with xUnit to validate its architecture automatically. These tests ensure:

-   **Module Boundaries**: Modules only depend on `*.Contracts` projects
-   **CQRS Compliance**: All commands, queries, and handlers follow naming conventions
-   **Layer Encapsulation**: Data and Domain layers remain internal to their modules
-   **Naming Conventions**: Consistent naming across the codebase
-   **Database Isolation**: Each module has its own DbContext

## Running Architecture Tests

Execute the architecture tests before committing code:

```bash
dotnet test tests/MentorSync.ArchitectureTests/
```

Expected output with all 32 tests passing:

```
MentorSync.ArchitectureTests test succeeded
Test summary: total: 32, failed: 0, succeeded: 32, skipped: 0
```

## Test Structure

Tests are organized into 4 categories:

### 1. CqrsPatternTests (7 tests)

Validates the Custom CQRS pattern used throughout MentorSync:

-   **Commands follow ICommand interface**: All write operations implement `ICommand<T>`
-   **Queries follow IQuery interface**: All read operations implement `IQuery<T>`
-   **CommandHandlers exist and are named correctly**: Handlers end with `CommandHandler`
-   **QueryHandlers exist and are named correctly**: Handlers end with `QueryHandler`
-   **Handlers are registered in DI**: Module registration includes handler mappings
-   **Endpoints are registered**: All IEndpoint implementations are mapped in Program.cs
-   **Validators exist for commands**: Each command has a FluentValidation validator

### 2. LayerArchitectureTests (7 tests)

Enforces proper layer separation within and across modules:

-   **Endpoints are public**: API entry points are accessible
-   **Commands and Queries are public**: CQRS operations are exposed
-   **Features are public entry points**: Feature implementations are accessible for extension
-   **Data layer is internal**: DbContext and configurations are hidden
-   **Domain layer is internal**: Entities and aggregates are internal
-   **Infrastructure is internal**: Internal services are not exposed
-   **Handlers don't cross module boundaries**: Handlers use only their module's DbContext

### 3. NamingConventionTests (10 tests)

Validates consistent naming patterns:

-   **Commands end with 'Command'**: e.g., `CreateUserCommand`
-   **Queries end with 'Query'**: e.g., `GetUserQuery`
-   **CommandHandlers end with 'CommandHandler'**: e.g., `CreateUserCommandHandler`
-   **QueryHandlers end with 'QueryHandler'**: e.g., `GetUserQueryHandler`
-   **Endpoints implement IEndpoint**: Minimal API handlers use IEndpoint pattern
-   **Validators end with 'Validator'**: e.g., `CreateUserCommandValidator`
-   **DbContexts end with 'DbContext'**: e.g., `UsersDbContext`
-   **Interfaces start with 'I'**: e.g., `IUserService`
-   **Request DTOs end with 'Request'**: e.g., `CreateUserRequest`
-   **Response DTOs end with 'Response'**: e.g., `UserProfileResponse`

### 4. ModularMonolithTests (11 tests)

Validates the modular monolith architecture constraints:

-   **Module data layers are well-organized**: Each module has isolated DbContext
-   **Module domain layers are well-organized**: Domain entities are properly structured
-   **Contracts projects are the only public interface**: No other module layers are public
-   **Feature commands and queries are public**: CQRS operations are accessible
-   **Each module has isolated DbContext**: Database access is module-scoped
-   **Module assemblies depend only on Contracts**: NO direct module-to-module references (Critical constraint)
-   **Module data layers are not accessible from other modules**: Data isolation enforced
-   **Module features don't cross module dependencies**: Feature isolation validated
-   **Data access handlers use module DbContext**: Handlers use correct database context
-   **Features follow vertical slice architecture**: Command/query bundling pattern
-   **Each module has module registration**: DI configuration is present

## Critical Architecture Constraint: Contracts-Only Dependencies

The most important architecture rule is enforced by `ModuleAssembliesShouldDependOnlyOnContracts`:

**Modules must ONLY depend on `*.Contracts` from other modules, NEVER on:**

-   `*.Data` - Database layer
-   `*.Domain` - Domain entities and aggregates
-   `*.Features` - Feature implementations
-   `*.Infrastructure` - Internal services

### Why This Matters

This constraint enables:

1. **Clear Module Boundaries**: Prevents "spagetti code" where modules know too much about each other
2. **Independent Deployment**: Modules can be deployed separately once extracted to microservices
3. **Testability**: Contracts enable easy mocking of inter-module interactions
4. **Maintainability**: Changes to one module don't ripple through the entire codebase

### Example Violation

If the Materials module directly references Users module classes:

```csharp
// ❌ FORBIDDEN - Materials module
using MentorSync.Users; // Violates architecture constraint
using MentorSync.Users.Data;

public class MaterialHandler
{
    private readonly UsersDbContext _context; // ❌ Direct access to Data layer
}
```

Test failure:

```
Users module assembly should only depend on Contracts from other modules
but has dependency on MentorSync.Users.Data
```

### Correct Approach

Use the Users Contracts:

```csharp
// ✅ CORRECT - Materials module
using MentorSync.Users.Contracts; // Only Contracts!

public class MaterialHandler
{
    private readonly IUsersService _usersService; // Use contract interface
}
```

## Test Implementation Details

### Architecture Loading

`ArchUnitExtensions.cs` dynamically loads all module assemblies:

-   **API Assembly**: `MentorSync.API.dll`
-   **Module Assemblies**: Discovered from `bin` directory
    -   `MentorSync.Users.dll`
    -   `MentorSync.Materials.dll`
    -   `MentorSync.Scheduling.dll`
    -   `MentorSync.Ratings.dll`
    -   `MentorSync.Recommendations.dll`
    -   `MentorSync.Notifications.dll`
-   **Contract Assemblies**: `MentorSync.{Module}.Contracts.dll`

### Test Philosophy

Tests validate **real architectural constraints** without masking violations:

-   ❌ **NOT USED**: `.WithoutRequiringPositiveResults()` hides failing rules
-   ✅ **CORRECT**: Tests fail when constraints are violated
-   ✅ **CORRECT**: Tests pass only when architecture is correct

## Continuous Integration

Architecture tests are automatically run in CI/CD:

1. **Before commit** (developer responsibility): `dotnet test tests/MentorSync.ArchitectureTests/`
2. **On pull request**: Architecture tests must pass for merge
3. **On main branch**: Architecture tests validate production code

## Common Architecture Violations and Fixes

### Violation 1: Direct Module Reference

**Problem**:

```csharp
// ❌ In Scheduling module
using MentorSync.Materials; // Wrong - should use Contracts

var material = new Material(); // Direct access to domain entity
```

**Fix**:

```csharp
// ✅ In Scheduling module
using MentorSync.Materials.Contracts;

var material = await _materialsService.GetMaterialAsync(id);
```

### Violation 2: Cross-Module DbContext Access

**Problem**:

```csharp
// ❌ In Ratings module
public RatingHandler(UsersDbContext dbContext) // Wrong module's DbContext
{
    _dbContext = dbContext;
}
```

**Fix**:

```csharp
// ✅ In Ratings module
public RatingHandler(RatingsDbContext dbContext) // Use your module's DbContext
{
    _dbContext = dbContext;
}
```

### Violation 3: Accessing Data Layer from Other Module

**Problem**:

```csharp
// ❌ In Materials module
using MentorSync.Recommendations.Data; // Accessing internal Data layer

public class MaterialHandler
{
    private readonly RecommendationsDbContext _context;
}
```

**Fix**:

```csharp
// ✅ In Materials module
using MentorSync.Recommendations.Contracts;

public class MaterialHandler
{
    private readonly IRecommendationsService _service;
}
```

## Extending Architecture Tests

To add new architecture validation rules:

1. **Understand ArchUnitNET API**: Review [ArchUnitNET documentation](https://archunitnet.readthedocs.io/)
2. **Add test method** to appropriate test class (CqrsPatternTests, LayerArchitectureTests, etc.)
3. **Use real architecture**: Don't mask violations with `.WithoutRequiringPositiveResults()`
4. **Validate your test**: Run `dotnet test` and ensure it catches real violations
5. **Document the rule**: Add comments explaining the constraint

Example:

```csharp
[Fact]
public void MyNewArchitectureRule()
{
    var rule = Classes()
        .That()
        .ResideInNamespace("MentorSync..*") // Match specific namespace
        .Should()
        .NotDependOnAny("System.Reflection") // Define the constraint
        .Because("we want to avoid reflection in business logic");

    rule.Check(_architecture); // Validates against loaded architecture
}
```

## Troubleshooting

### Tests are passing but violations exist

This indicates tests aren't strict enough. Review the constraint and make it more specific.

### Tests are failing during development

This is expected when refactoring. Fix the architecture violation or update the rule if the architecture has legitimately changed.

### New module isn't being tested

Ensure:

1. Module assembly is in `bin` directory
2. `ArchUnitExtensions.cs` discovers it (check assembly naming)
3. Test methods include the module's namespace patterns

## Related Documentation

-   **Main Architecture**: `docs/Architecture.md`
-   **CQRS Pattern**: `.github/copilot-instructions.md` (Custom CQRS section)
-   **Module Communication**: `docs/Architecture.md` (Contracts-Only Pattern section)
-   **Development Guidelines**: `.github/copilot-instructions.md` (Modular Monolith Architecture section)

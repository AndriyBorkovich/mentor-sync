# MentorSync Architecture

## Architecture Pattern: Modular Monolith

Chosen pattern: [Modular Monolith](https://www.milanjovanovic.tech/blog/what-is-a-modular-monolith)

The system is architected as a **modular monolith** - a single deployable application composed of independent, loosely-coupled feature modules. This approach provides the organizational benefits of microservices while maintaining the operational simplicity of a monolith, with a clear migration path to microservices if needed in the future.

## Database Architecture

**Db approach**: Separate schema per module

Each module has its own PostgreSQL schema, ensuring:

-   **Database isolation**: Modules cannot accidentally share data at the database level
-   **Independent evolution**: Schema changes in one module don't affect others
-   **Clear data ownership**: Each module owns its data domain exclusively

## Module Communication: Contracts-Only Pattern

**Critical Architectural Rule**: Modules communicate **ONLY** through `*.Contracts` projects. Modules must **NEVER** directly reference another module's implementation.

### Allowed Dependencies

```
Module A → Module B.Contracts ✅
Module A → Module B ✅ (if B is the core API assembly)
```

### Forbidden Dependencies

```
Module A → Module B.Data ❌
Module A → Module B.Domain ❌
Module A → Module B.Features ❌
Module A → Module B.Infrastructure ❌
```

### Why Contracts-Only?

1. **Clear Module Boundaries**: Contracts define the public interface, hiding implementation details
2. **Loose Coupling**: Modules can evolve independently as long as Contracts don't change
3. **Scalability Path**: Individual modules can be extracted to microservices by replacing contract dependencies with HTTP calls
4. **Testability**: Contracts enable easy mocking and testing of inter-module interactions
5. **Maintainability**: Forces developers to think about module responsibilities and boundaries

### Implementation Pattern

Each module includes a corresponding `*.Contracts` project:

```
Modules/
├── Users/
│   ├── MentorSync.Users.Contracts/
│   │   └── (public interfaces, DTOs, events)
│   ├── MentorSync.Users.Data/
│   ├── MentorSync.Users.Domain/
│   └── MentorSync.Users.Features/
├── Materials/
│   ├── MentorSync.Materials.Contracts/
│   ├── MentorSync.Materials.Data/
│   ├── MentorSync.Materials.Domain/
│   └── MentorSync.Materials.Features/
```

The `*.Contracts` project contains:

-   **Service Interfaces**: Contracts that other modules use to interact with this module
-   **Data Transfer Objects (DTOs)**: Immutable data structures for inter-module communication
-   **Domain Events**: Events published by the module for other modules to subscribe to
-   **Exceptions**: Custom exceptions thrown by the module

### Example: Materials Module Depending on Users

**Allowed** ✅:

```csharp
// In Materials module
using MentorSync.Users.Contracts;

public class MaterialHandler
{
    private readonly IUsersService _usersService; // From Users.Contracts

    public async Task<Result> CreateMaterial(CreateMaterialCommand command)
    {
        var user = await _usersService.GetUserAsync(command.UserId);
        // ...
    }
}
```

**NOT Allowed** ❌:

```csharp
// In Materials module
using MentorSync.Users.Data;    // ❌ FORBIDDEN
using MentorSync.Users.Domain;  // ❌ FORBIDDEN

public class MaterialHandler
{
    private readonly UsersDbContext _dbContext;  // ❌ NO DIRECT ACCESS
}
```

## Architecture Validation

All architectural constraints are **validated by automated tests** in `tests/MentorSync.ArchitectureTests/`:

-   **Contract-Only Dependencies**: Ensures modules only reference `*.Contracts` projects
-   **Data Layer Isolation**: Verifies each module has its own isolated DbContext
-   **CQRS Pattern Compliance**: Validates all commands/queries follow naming conventions
-   **Layer Encapsulation**: Enforces that internal layers are not accessed from outside the module
-   **Naming Conventions**: Checks all types follow established patterns

**Before committing code**, run the architecture tests to ensure no violations:

```bash
dotnet test tests/MentorSync.ArchitectureTests/
```

Violations of architecture constraints will cause test failures and block commits.

---

## Main Modules

-   [Users Module](../Modules/Users%20module.md)
-   [Materials Module](../Modules/Materials%20module.md)
-   [Scheduling Module](../Modules/Scheduling%20(booking)%20module.md>)
-   [Ratings Module](../Modules/Ratings%20module.md)
-   [Recommendations Module](../Modules/Recommendations%20module.md)
-   [Notifications Module](../Modules/Notifications%20module.md)

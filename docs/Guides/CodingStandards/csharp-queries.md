# Backend Queries Style Guide

Queries represent read-only operations in the Custom CQRS pattern. They must be immutable records implementing `IQuery<T>`.

## Location & Naming

-   **Location**: `src/Modules/{Domain}/Features/{Feature}/{Feature}Query.cs`
-   **Naming**: `Get{Entity}{Criteria}Query` (e.g., `GetUserProfileQuery`, `GetMaterialsByCategoryQuery`)
-   **Example**: `src/Modules/Users/Features/GetUserProfile/GetUserProfileQuery.cs`

## Query Pattern

```cs
namespace MentorSync.Modules.Users.Features.GetUserProfile;

using MentorSync.SharedKernel.Abstractions.CQRS;

/// <summary>
/// Retrieves a user profile by user ID.
/// </summary>
public sealed record GetUserProfileQuery(string UserId) : IQuery<UserProfileResponse>;
```

## Required Elements

1. **File-scoped namespace**: `namespace Path.To.Feature;`
2. **Sealed record**: `public sealed record QueryName(...) : IQuery<T>;`
3. **Immutable parameters**: All as positional parameters
4. **Implements IQuery<T>**: Generic type = response DTO
5. **XML docs**: `<summary>` explaining what is retrieved
6. **Proper naming**: `Get` + entity description

## Complex Queries with Filters

For list queries supporting pagination and filtering:

```cs
namespace MentorSync.Modules.Materials.Features.ListMaterials;

using MentorSync.SharedKernel.Abstractions.CQRS;

/// <summary>
/// Retrieves paginated list of materials with optional filtering and searching.
/// </summary>
public sealed record GetMaterialsQuery(
    int PageNumber = 1,
    int PageSize = 12,
    string? SearchTerm = null,
    string? Category = null,
    int? MinDifficultyLevel = null,
    int? MaxDifficultyLevel = null
) : IQuery<PagedResponse<MaterialResponse>>;

/// <summary>
/// Represents a paginated response for list operations.
/// </summary>
public sealed record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);
```

## Query Parameters

-   **Default values**: Provide sensible defaults (PageNumber=1, PageSize=12)
-   **Optional filters**: Use `?` for optional criteria
-   **Clear naming**: `SearchTerm`, `Category`, not just `filter` or `query`
-   **Type-safe**: Use enums for categorical filters, not strings

Better approach with enums:

```cs
public enum MaterialCategory
{
    Programming,
    Design,
    Business,
    PersonalDevelopment,
}

public sealed record GetMaterialsQuery(
    int PageNumber = 1,
    int PageSize = 12,
    string? SearchTerm = null,
    MaterialCategory? Category = null,
    DifficultyLevel? MinDifficulty = null
) : IQuery<PagedResponse<MaterialResponse>>;
```

## Specialized Query Examples

**Single entity retrieval**:

```cs
public sealed record GetMentorQuery(string MentorId) : IQuery<MentorResponse>;
```

**List with search**:

```cs
public sealed record SearchMentorsQuery(
    string? SearchTerm,
    string[]? Expertise,
    int PageNumber = 1,
    int PageSize = 12
) : IQuery<PagedResponse<MentorCardResponse>>;
```

**Related entities**:

```cs
public sealed record GetSessionWithMessagesQuery(
    string SessionId
) : IQuery<SessionDetailResponse>;
```

**Aggregated data**:

```cs
public sealed record GetUserDashboardStatsQuery(
    string UserId
) : IQuery<DashboardStatsResponse>;
```

## Conventions

| Operation         | Pattern                        | Example                      |
| ----------------- | ------------------------------ | ---------------------------- |
| Get single        | `Get{Entity}Query`             | `GetUserQuery`               |
| Get with criteria | `Get{Entity}By{Criteria}Query` | `GetMentorsByExpertiseQuery` |
| List/search       | `Get{Entities}Query`           | `GetMaterialsQuery`          |
| Aggregation       | `Get{Entity}{Metric}Query`     | `GetUserSessionCountQuery`   |

## File Organization

```
Features/
├── GetUserProfile/
│   ├── GetUserProfileQuery.cs           // Record definition
│   ├── GetUserProfileQueryHandler.cs    // Business logic
│   ├── GetUserProfileResponse.cs        // Response DTO
│   └── GetUserProfileEndpoint.cs        // Route mapping
└── GetMaterials/
    └── ...
```

## No Validation in Queries

Queries don't require validators. Validation happens at endpoints:

```cs
// In endpoint
if (query.PageNumber < 1) return Results.BadRequest("Page must be >= 1");
```

Or use `ArgumentException` in handler:

```cs
public async Task<Result<PagedResponse<MaterialResponse>>> Handle(
    GetMaterialsQuery query,
    CancellationToken cancellationToken = default)
{
    if (query.PageNumber < 1 || query.PageSize < 1)
    {
        return Result.Invalid(new ValidationError("pagination", "Invalid page or size"));
    }

    // ... fetch and return
}
```

## Performance Considerations

```cs
// Query with explicit fields to select (don't fetch unnecessary data)
public sealed record GetMentorCardQuery(string MentorId) : IQuery<MentorCardResponse>;

// Handler uses direct DbContext with projection to only load needed fields
var mentor = await dbContext.Users
    .AsNoTracking()  // Read-only, don't track
    .Where(u => u.Id == query.MentorId)
    .Select(u => new MentorCardResponse(
        u.Id,
        u.FirstName,
        u.LastName,
        u.AverageRating
    ))
    .FirstOrDefaultAsync(cancellationToken);
```

## Testing Example

```cs
[TestClass]
public class GetUserProfileQueryTests
{
    [TestMethod]
    public async Task Handle_WithValidUserId_ReturnsUserProfile()
    {
        // Arrange
        var query = new GetUserProfileQuery(userId: "user-123");
        var dbContext = new UsersDbContext(...);
        var handler = new GetUserProfileQueryHandler(dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("user-123", result.Value.Id);
    }

    [TestMethod]
    public async Task Handle_WithInvalidUserId_ReturnsNotFound()
    {
        // Arrange
        var query = new GetUserProfileQuery(userId: "non-existent");
        var dbContext = new UsersDbContext(...);
        var handler = new GetUserProfileQueryHandler(dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.Status == ResultStatus.NotFound);
    }
}
```

## Best Practices

-   ✅ Use `AsNoTracking()` for read-only queries (performance)
-   ✅ Project to specific DTOs (not full entities)
-   ✅ Include pagination for list operations
-   ✅ Use enums for categorical filters (type safety)
-   ✅ Provide sensible defaults for optional parameters
-   ✅ Include XML docs explaining query purpose
-   ✅ Name clearly to indicate what is retrieved
-   ✅ Inject DbContext directly (no repositories)
-   ✅ Use LINQ directly for data access
-   ❌ Don't modify state in query handlers
-   ❌ Don't use queries for business logic computation
-   ❌ Never fetch unnecessary related entities
-   ❌ Avoid N+1 query problems (use Include or Select projection)
-   ❌ Don't use repositories (query DbContext directly)

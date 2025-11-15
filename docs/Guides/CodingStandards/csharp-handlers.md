# Backend Handlers Style Guide

Handlers contain business logic and execute commands/queries in the Custom CQRS pattern.

## Command Handler Pattern

```cs
namespace MentorSync.Modules.Users.Features.Register;

using MentorSync.SharedKernel.Abstractions.CQRS;
using Ardalis.Result;

/// <summary>
/// Handles user registration with email, password, and basic profile info.
/// </summary>
public sealed class RegisterCommandHandler(
    UsersDbContext dbContext,
    IJwtService jwtService,
    IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterCommand, AuthResponse>
{
    /// <summary>
    /// Handles the registration command and creates a new user account.
    /// </summary>
    /// <param name="command">The registration command with user details.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>AuthResponse with access and refresh tokens on success.</returns>
    public async Task<Result<AuthResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Check if user already exists using direct DbContext query
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);
        if (existingUser is not null)
        {
            return Result.Conflict("User with this email already exists");
        }

        // 2. Hash password
        var passwordHash = passwordHasher.HashPassword(command.Password);

        // 3. Create new user entity
        var newUser = new User(
            id: Guid.NewGuid().ToString(),
            email: command.Email,
            firstName: command.FirstName,
            lastName: command.LastName,
            passwordHash: passwordHash,
            createdAt: DateTime.UtcNow
        );

        // 4. Save to database
        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        // 5. Generate tokens
        var tokens = jwtService.GenerateTokens(newUser);

        // 6. Return success
        return Result.Success(new AuthResponse(tokens.AccessToken, tokens.RefreshToken));
    }
}
```

## Query Handler Pattern

```cs
namespace MentorSync.Modules.Users.Features.GetUserProfile;

using MentorSync.SharedKernel.Abstractions.CQRS;
using Ardalis.Result;

/// <summary>
/// Handles retrieval of user profile information.
/// </summary>
public sealed class GetUserProfileQueryHandler(
    UsersDbContext dbContext)
    : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    /// <summary>
    /// Retrieves user profile data for the specified user ID.
    /// </summary>
    /// <param name="query">Query containing the user ID to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>UserProfileResponse with user details on success, NotFound if user doesn't exist.</returns>
    public async Task<Result<UserProfileResponse>> Handle(
        GetUserProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        // Direct DbContext query with AsNoTracking for read-only operations
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            return Result.NotFound($"User with ID '{query.UserId}' not found");
        }

        var response = UserProfileResponse.FromUser(user);
        return Result.Success(response);
    }
}
```

## Key Principles

1. **Single Responsibility**: Each handler does one thing
2. **Async All The Way**: All I/O operations are `async`
3. **Validation First**: Check inputs before business logic
4. **Domain Logic**: Enforce business rules in handlers
5. **Direct DbContext Access**: Query DbContext directly with LINQ
6. **Result Pattern**: Always return `Result<T>` variants
7. **Immutable State**: No handlers modify handler fields
8. **Cancellation Support**: Accept and pass `CancellationToken`
9. **Query Optimization**: Use `AsNoTracking()` for read-only queries
10. **DTO Projection**: Use `Select()` to project to DTOs instead of loading full entities

## Error Handling in Handlers

```cs
public async Task<Result<MaterialResponse>> Handle(
    UpdateMaterialCommand command,
    CancellationToken cancellationToken = default)
{
    // Validation
    if (string.IsNullOrWhiteSpace(command.Title))
    {
        return Result.Invalid(new ValidationError(
            nameof(command.Title),
            "Title is required"));
    }

    // Check entity exists using direct DbContext query
    var material = await dbContext.Materials
        .FirstOrDefaultAsync(m => m.Id == command.MaterialId, cancellationToken);
    if (material is null)
    {
        return Result.NotFound($"Material with ID '{command.MaterialId}' not found");
    }

    // Check authorization
    if (material.CreatedByUserId != command.RequestingUserId)
    {
        return Result.Forbidden("You cannot update materials created by other users");
    }

    // Business logic
    material.Update(command.Title, command.Description, command.Category);

    // Persist - no update method needed, just SaveChangesAsync
    dbContext.Materials.Update(material);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Result.Success(MaterialResponse.FromMaterial(material));
}
```

## Handler Dependencies

Use constructor injection (primary constructor pattern preferred):

```cs
public sealed class CreateSessionCommandHandler(
    SchedulingDbContext dbContext,
    INotificationService notificationService,
    ILogger<CreateSessionCommandHandler> logger)
    : ICommandHandler<CreateSessionCommand, SessionResponse>
    public async Task<Result<SessionResponse>> Handle(
        CreateSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating session for mentor {MentorId}", command.MentorId);

        try
        {
            // Direct DbContext query
            var mentor = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == command.MentorId, cancellationToken);
            if (mentor is null)
            {
                _logger.LogWarning("Mentor not found: {MentorId}", command.MentorId);
                return Result.NotFound("Mentor not found");
            }

            // Create session logic...

            _logger.LogInformation("Session created successfully: {SessionId}", session.Id);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session");
            return Result.Error("Failed to create session");
        }
    }
}
```

## Logging in Handlers

```cs
public async Task<Result<SessionResponse>> Handle(
    CreateSessionCommand command,
    CancellationToken cancellationToken = default)
{
    logger.LogInformation("Creating session for mentor {MentorId}", command.MentorId);

    try
    {
        // Direct DbContext query
        var mentor = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == command.MentorId, cancellationToken);
        if (mentor is null)
        {
            logger.LogWarning("Mentor not found: {MentorId}", command.MentorId);
            return Result.NotFound("Mentor not found");
        }

        // Create session logic...

        logger.LogInformation("Session created successfully: {SessionId}", session.Id);
        return Result.Success(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating session");
        return Result.Error("Failed to create session");
    }
}
```

## Pagination Handling

```cs
public async Task<Result<PagedResponse<MaterialResponse>>> Handle(
    GetMaterialsQuery query,
    CancellationToken cancellationToken = default)
{
    var materials = dbContext.Materials.AsQueryable();

    // Apply filters
    if (!string.IsNullOrWhiteSpace(query.SearchTerm))
    {
        materials = materials.Where(m => m.Title.Contains(query.SearchTerm));
    }

    if (query.Category.HasValue)
    {
        materials = materials.Where(m => m.Category == query.Category.Value);
    }

    // Count total before pagination
    var totalCount = await materials.CountAsync(cancellationToken);

    // Apply pagination
    var items = await materials
        .Skip((query.PageNumber - 1) * query.PageSize)
        .Take(query.PageSize)
        .OrderByDescending(m => m.CreatedAt)
        .AsNoTracking()
        .Select(m => MaterialResponse.FromMaterial(m))
        .ToListAsync(cancellationToken);

    // Build response
    var response = new PagedResponse<MaterialResponse>(
        Items: items,
        TotalCount: totalCount,
        PageNumber: query.PageNumber,
        PageSize: query.PageSize,
        TotalPages: (totalCount + query.PageSize - 1) / query.PageSize,
        HasNextPage: query.PageNumber < ((totalCount + query.PageSize - 1) / query.PageSize),
        HasPreviousPage: query.PageNumber > 1
    );

    return Result.Success(response);
}
```

## Testing Handlers

```cs
[TestClass]
public class RegisterCommandHandlerTests
{
    private UsersDbContext _dbContext;
    private Mock<IJwtService> _mockJwtService;
    private Mock<IPasswordHasher> _mockPasswordHasher;
    private RegisterCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        // Use in-memory DbContext for testing
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _dbContext = new UsersDbContext(options);

        _mockJwtService = new Mock<IJwtService>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _handler = new RegisterCommandHandler(
            _dbContext,
            _mockJwtService.Object,
            _mockPasswordHasher.Object
        );
    }

    [TestMethod]
    public async Task Handle_WithValidCommand_ReturnsAuthResponse()
    {
        // Arrange
        var command = new RegisterCommand(
            Email: "test@example.com",
            Password: "SecurePass123",
            FirstName: "John",
            LastName: "Doe"
        );

        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtService = new Mock<IJwtService>();

        mockPasswordHasher.Setup(x => x.HashPassword(command.Password))
            .Returns("hashed_password");
        mockJwtService.Setup(x => x.GenerateTokens(It.IsAny<User>()))
            .Returns(new TokenPair("access_token", "refresh_token"));

        var handler = new RegisterCommandHandler(
            _dbContext,
            mockJwtService.Object,
            mockPasswordHasher.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
        var userInDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
        Assert.IsNotNull(userInDb);
    }

    [TestMethod]
    public async Task Handle_WithExistingEmail_ReturnsConflict()
    {
        // Arrange
        var existingUser = new User("1", "existing@example.com", "John", "Doe", "hash", DateTime.UtcNow);
        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        var command = new RegisterCommand("existing@example.com", "Pass123", "John", "Doe");
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtService = new Mock<IJwtService>();

        var handler = new RegisterCommandHandler(
            _dbContext,
            mockJwtService.Object,
            mockPasswordHasher.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ResultStatus.Conflict, result.Status);
    }
}
```

## Best Practices

-   ✅ Keep handlers focused on single operation
-   ✅ Validate inputs before business logic
-   ✅ Use DbContext directly for data access
-   ✅ Return appropriate Result variants
-   ✅ Support cancellation tokens
-   ✅ Log important operations
-   ✅ Handle exceptions gracefully
-   ✅ Use in-memory DbContext for testing
-   ✅ Verify database state after operations
-   ✅ Use dependency injection
-   ❌ Don't query DbContext directly
-   ❌ Don't throw exceptions to client
-   ❌ Don't mix validation concerns
-   ❌ Don't ignore cancellation tokens

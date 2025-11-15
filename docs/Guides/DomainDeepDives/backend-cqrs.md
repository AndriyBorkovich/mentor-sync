# Backend API & CQRS Domain Deep Dive

## Overview

Backend uses ASP.NET Core with a **modular monolith architecture**. Each domain module (Users, Materials, Scheduling, Ratings, Recommendations, Notifications) is self-contained with its own features, DbContext, and API endpoints. All modules use a custom CQRS pattern (ICommand/IQuery), Minimal APIs via IEndpoint interface, and Result<T> pattern for all operations. Recently migrated from MediatR to lightweight custom implementation.

## Modular Monolith Structure

Each module in `src/Modules/{Domain}/` follows this pattern:

```
src/Modules/Users/                    # Example module
├── MentorSync.Users/
│   ├── Features/
│   │   ├── Login/
│   │   │   ├── LoginCommand.cs
│   │   │   ├── LoginCommandHandler.cs
│   │   │   ├── LoginCommandValidator.cs
│   │   │   ├── LoginEndpoint.cs
│   │   │   ├── LoginRequest.cs
│   │   │   └── LoginResponse.cs
│   │   └── GetUserProfile/
│   │       ├── GetUserProfileQuery.cs
│   │       ├── GetUserProfileQueryHandler.cs
│   │       ├── GetUserProfileEndpoint.cs
│   │       └── GetUserProfileResponse.cs
│   ├── Data/
│   │   ├── UsersDbContext.cs         # Isolated DbContext for Users
│   │   └── Configurations/
│   │       ├── UserConfiguration.cs
│   │       └── UserSkillConfiguration.cs
│   ├── Domain/
│   │   ├── User.cs                  # User entity
│   │   ├── UserSkill.cs
│   │   └── Events/
│   │       └── UserRegisteredEvent.cs
│   ├── Infrastructure/
│   │   └── Services/
│   │       └── JwtService.cs
│   ├── ModuleRegistration.cs         # DI setup for this module
│   └── MentorSync.Users.csproj
└── MentorSync.Users.Contracts/       # Public API for other modules
    ├── Models/
    │   └── UserDto.cs
    └── Services/
        └── IUserService.cs
```

### Key Principles

1. **Isolated DbContext**: Each module has its own PostgreSQL schema (e.g., `users`, `materials`, `scheduling`)
2. **Self-Contained Features**: All CQRS operations for a feature in one folder
3. **Auto-Registered Endpoints**: IEndpoint implementations discovered via reflection
4. **Module Contracts**: Public interfaces exposed for cross-module communication
5. **Shared Infrastructure**: All modules depend on `MentorSync.SharedKernel`

## Custom CQRS Pattern

### Command Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Messaging/Commands/ICommand.cs
namespace MentorSync.SharedKernel.Abstractions.Messaging;

public interface ICommand<TResponse>
{
    // Marker interface for command typing
}
```

### Command Handler Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Messaging/Handlers/Command/ICommandHandler.cs
namespace MentorSync.SharedKernel.Abstractions.Messaging.Handlers.Command;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
```

### Query Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Messaging/Queries/IQuery.cs
namespace MentorSync.SharedKernel.Abstractions.Messaging.Queries;

public interface IQuery<TResponse>
{
    // Marker interface for query typing
}
```

### Query Handler Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Messaging/Handlers/Query/IQueryHandler.cs
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
```

### Mediator Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Messaging/IMediator.cs
public interface IMediator
{
    Task<Result<TResponse>> SendCommandAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>;

    Task<Result<TResponse>> SendQueryAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>;
}
```

## Example Command Implementation

### Login Command (Users Module)

```cs
// src/Modules/Users/MentorSync.Users/Features/Login/LoginCommand.cs
namespace MentorSync.Users.Features.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
```

### Login Command Handler

```cs
// src/Modules/Users/MentorSync.Users/Features/Login/LoginCommandHandler.cs
public sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    UsersDbContext usersDbContext,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions,
    ILogger<LoginCommandHandler> logger)
    : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email;
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.NotFound("User not found");
        }

        var passwordCheckResult = await CheckPasswordAsync(user, command.Password);
        if (!passwordCheckResult.IsSuccess)
        {
            return passwordCheckResult;
        }

        var tokenResult = await jwtTokenService.GenerateToken(user);
        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);

        var userRoles = await userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? Roles.Admin;

        var needsOnboarding = role switch
        {
            Roles.Mentee => !await usersDbContext.MenteeProfiles.AnyAsync(mp => mp.MenteeId == user.Id, cancellationToken),
            Roles.Mentor => !await usersDbContext.MentorProfiles.AnyAsync(mp => mp.MentorId == user.Id, cancellationToken),
            _ => false
        };

        logger.LogInformation("User {UserId} logged in successfully", user.Id);

        return Result.Success(new AuthResponse(
            tokenResult.AccessToken,
            tokenResult.RefreshToken,
            tokenResult.Expiration,
            needsOnboarding));
    }
}
```

## Example Query Implementation

### GetUserProfile Query

```cs
// src/Modules/Users/MentorSync.Users/Features/GetUserProfile/GetUserProfileQuery.cs
public sealed record GetUserProfileQuery(int UserId) : IQuery<UserProfileResponse>;
```

### Query Handler

```cs
// src/Modules/Users/MentorSync.Users/Features/GetUserProfile/GetUserProfileQueryHandler.cs
public sealed class GetUserProfileQueryHandler(
    UsersDbContext dbContext)
    : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    public async Task<Result<UserProfileResponse>> Handle(
        GetUserProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        // Direct DbContext query
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        if (user is null)
        {
            return Result.NotFound("User not found");
        }

        return Result.Success(new UserProfileResponse(
            user.Id,
            user.UserName,
            user.Email,
            user.Role));
    }
}
```

## Minimal API Endpoints

### IEndpoint Interface

```cs
// src/MentorSync.SharedKernel/Abstractions/Endpoints/IEndpoint.cs
namespace MentorSync.SharedKernel.Abstractions.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

### Example Login Endpoint

```cs
// src/Modules/Users/MentorSync.Users/Features/Login/LoginEndpoint.cs
public sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", Handle)
            .WithTags(TagsConstants.Users)
            .WithDescription("User login endpoint")
            .Produces<AuthResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> Handle(
        LoginRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}
```

### Query Endpoint Example

```cs
// src/Modules/Users/MentorSync.Users/Features/GetUserProfile/GetUserProfileEndpoint.cs
public sealed class GetUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", Handle)
            .WithTags(TagsConstants.Users)
            .WithDescription("Get user profile")
            .Produces<UserProfileResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        IMediator mediator,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var query = new GetUserProfileQuery(userId);

        var result = await mediator.SendQueryAsync<GetUserProfileQuery, UserProfileResponse>(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound();
    }
}
```

## Result Pattern

### Result<T> Type

```cs
// From Ardalis.Result library
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IEnumerable<string> Errors { get; }
    public string? SuccessMessage { get; }

    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Error(string error) => new(false, null, [error]);
    public static Result<T> NotFound(string message) => new(false, null, [message], ResultStatus.NotFound);
}
```

### Result Usage in Commands

```cs
// Validation failure
if (string.IsNullOrWhiteSpace(command.Email))
{
    return Result.Error("Email is required");
}

// Not found
var user = await userManager.FindByEmailAsync(command.Email);
if (user is null)
{
    return Result.NotFound("User not found");
}

// Success
return Result.Success(new AuthResponse(...));
```

## Endpoint Registration

### Program.cs Extension

```cs
// src/MentorSync.API/Program.cs
using MentorSync.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsMetadata();
builder.AddApplicationModules();

var app = builder.Build();
app.MapEndpoints();
await app.RunAsync();
```

### MapEndpoints Extension

```cs
// src/MentorSync.API/Extensions/HostingExtensions.cs
internal static void MapEndpoints(this WebApplication app)
{
    var endpointAssemblies = new[] { typeof(Program).Assembly };

    var endpoints = endpointAssemblies
        .SelectMany(s => s.GetTypes())
        .Where(p => p.IsClass && !p.IsAbstract && typeof(IEndpoint).IsAssignableFrom(p))
        .Select(Activator.CreateInstance)
        .Cast<IEndpoint>();

    foreach (var endpoint in endpoints)
    {
        endpoint.MapEndpoint(app);
    }
}
```

## Key Patterns

1. **Command vs Query Separation**: Commands modify state, Queries read state
2. **Result Pattern**: All operations return Result<T> with success/error status
3. **IEndpoint Interface**: All routes registered via IEndpoint implementations
4. **Dependency Injection**: Handlers injected via constructor
5. **CancellationToken**: All async operations support cancellation
6. **No Controllers**: Pure Minimal API approach with route mappers
7. **Async/Await**: All I/O operations are async with ConfigureAwait(false)

## File Organization

```cs
src/Modules/Users/MentorSync.Users/
├── Features/
│   ├── Login/
│   │   ├── LoginCommand.cs
│   │   ├── LoginCommandHandler.cs
│   │   ├── LoginEndpoint.cs
│   │   └── LoginRequest.cs
│   ├── GetUserProfile/
│   │   ├── GetUserProfileQuery.cs
│   │   ├── GetUserProfileQueryHandler.cs
│   │   └── GetUserProfileEndpoint.cs
│   └── ...
├── Data/
│   └── UsersDbContext.cs
├── Domain/
│   └── User/
│       └── AppUser.cs
└── Infrastructure/
    └── JwtTokenService.cs
```

## Migration Notes

Recently migrated from **MediatR** to **Custom CQRS**:

-   Changed: `IRequest<Result<T>>` → `ICommand<T>` / `IQuery<T>`
-   Changed: `IRequestHandler<T, Result<U>>` → `ICommandHandler<T, U>` / `IQueryHandler<T, U>`
-   Changed: `ISender.Send()` → `IMediator.SendCommandAsync<>()` / `SendQueryAsync<>()`
-   Benefit: Lightweight, explicit, no external dependencies

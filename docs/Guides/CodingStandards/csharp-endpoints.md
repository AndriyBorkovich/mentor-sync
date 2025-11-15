# Backend Endpoints Style Guide

Endpoints map HTTP routes to handlers using the IEndpoint interface pattern.

## Endpoint Pattern

```cs
namespace MentorSync.Modules.Users.Features.Login;

using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Abstractions.CQRS;

/// <summary>
/// Endpoint for user login.
/// </summary>
public sealed class LoginEndpoint : IEndpoint
{
    /// <summary>
    /// Maps the login endpoint to the HTTP POST route.
    /// </summary>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", Handle)
            .WithName("UserLogin")
            .WithOpenApi()
            .AllowAnonymous()
            .Produces<AuthResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Handles the login request.
    /// </summary>
    private static async Task<IResult> Handle(
        LoginRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        // Map HTTP request to command
        var command = new LoginCommand(request.Email, request.Password);

        // Execute command
        var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(
            command,
            cancellationToken);

        // Map result to HTTP response
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { errors = result.Errors });
    }
}
```

## Protected Endpoint with Authorization

```cs
public sealed class UpdateUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{userId}/profile", Handle)
            .WithName("UpdateUserProfile")
            .WithOpenApi()
            .RequireAuthorization() // Requires [Authorize]
            .Produces<UserProfileResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(
        string userId,
        UpdateUserProfileRequest request,
        HttpContext httpContext,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        // Get authenticated user ID
        var currentUserId = httpContext.User.FindFirst("sub")?.Value;
        if (currentUserId is null)
        {
            return Results.Unauthorized();
        }

        // Check authorization (user can only update their own profile)
        if (currentUserId != userId)
        {
            return Results.Forbid();
        }

        var command = new UpdateUserProfileCommand(userId, request.FirstName, request.LastName, request.Bio);
        var result = await mediator.SendCommandAsync<UpdateUserProfileCommand, UserProfileResponse>(
            command,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Status switch
            {
                ResultStatus.NotFound => Results.NotFound(),
                _ => Results.BadRequest(result),
            };
    }
}
```

## Query Endpoint with Pagination

```cs
public sealed class GetMaterialsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/materials", Handle)
            .WithName("ListMaterials")
            .WithOpenApi()
            .Produces<PagedResponse<MaterialResponse>>(StatusCodes.Status200OK)
            .WithParameterValidation();
    }

    private static async Task<IResult> Handle(
        [AsParameters] GetMaterialsRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        // Validate pagination
        if (request.PageNumber < 1 || request.PageSize < 1)
        {
            return Results.BadRequest("Page and size must be >= 1");
        }

        var query = new GetMaterialsQuery(
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            SearchTerm: request.SearchTerm,
            Category: request.Category);

        var result = await mediator.SendQueryAsync<GetMaterialsQuery, PagedResponse<MaterialResponse>>(
            query,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result);
    }
}
```

## File Upload Endpoint

```cs
public sealed class UploadAvatarEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/files/upload/avatar", Handle)
            .WithName("UploadAvatar")
            .RequireAuthorization()
            .Produces<UploadAvatarResponse>(StatusCodes.Status200OK)
            .DisableAntiforgery(); // For multipart/form-data
    }

    private static async Task<IResult> Handle(
        HttpContext httpContext,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var userId = httpContext.User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        var file = httpContext.Request.Form.Files.FirstOrDefault("file");
        if (file is null || file.Length == 0)
        {
            return Results.BadRequest("No file provided");
        }

        // Validate file
        if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
        {
            return Results.BadRequest("Only JPEG and PNG files are allowed");
        }

        using var stream = file.OpenReadStream();
        var command = new UploadAvatarCommand(userId, file.FileName, stream);
        var result = await mediator.SendCommandAsync<UploadAvatarCommand, UploadAvatarResponse>(
            command,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result);
    }
}
```

## Key Aspects

1. **Routes**: All prefixed with `/api/`
2. **HTTP Methods**: GET (query), POST (create), PUT (update), DELETE (delete)
3. **Authorization**: Use `RequireAuthorization()` and `AllowAnonymous()`
4. **OpenAPI**: Add `WithOpenApi()` for Swagger documentation
5. **Results**: Map Result<T> status to HTTP status codes
6. **Cancellation**: Always pass `CancellationToken`

## Request DTOs

```cs
public sealed record LoginRequest(string Email, string Password);

public sealed record GetMaterialsRequest(
    int PageNumber = 1,
    int PageSize = 12,
    string? SearchTerm = null,
    string? Category = null
);

public sealed record UpdateUserProfileRequest(
    string FirstName,
    string LastName,
    string? Bio
);
```

## Response Mapping

```cs
// ✅ Good: Map Result status to HTTP
return result.Status switch
{
    ResultStatus.Success => Results.Ok(result.Value),
    ResultStatus.NotFound => Results.NotFound(),
    ResultStatus.Unauthorized => Results.Unauthorized(),
    ResultStatus.Forbidden => Results.Forbid(),
    ResultStatus.Invalid => Results.BadRequest(result),
    _ => Results.InternalServerError(),
};

// ❌ Bad: Throw exceptions
if (result.IsSuccess is false)
    throw new Exception("Operation failed");
```

## Testing Endpoints

```cs
[TestClass]
public class LoginEndpointTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "SecurePass123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/login", request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsAsync<AuthResponse>();
        Assert.IsNotNull(content.AccessToken);
    }
}
```

## Best Practices

-   ✅ One endpoint per IEndpoint class
-   ✅ Use [AsParameters] for query string bindings
-   ✅ Add OpenAPI documentation metadata
-   ✅ Validate input at endpoint level
-   ✅ Map Results to appropriate HTTP status codes
-   ✅ Support cancellation tokens
-   ✅ Handle authentication/authorization
-   ❌ Don't include business logic (use handlers)
-   ❌ Don't throw exceptions (return Results)
-   ❌ Don't skip authorization checks
-   ❌ Don't return entities directly (use DTOs)

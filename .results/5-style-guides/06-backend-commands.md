# Backend Commands Style Guide

Commands represent write operations in the Custom CQRS pattern. They must be immutable records implementing `ICommand<T>`.

## Location & Naming

-   **Location**: `src/Modules/{Domain}/Features/{Feature}/{Feature}Command.cs`
-   **Naming**: `{Action}{Entity}Command` (e.g., `CreateUserCommand`, `UpdateMaterialCommand`)
-   **Example**: `src/Modules/Users/Features/Register/RegisterCommand.cs`

## Command Pattern

```cs
namespace MentorSync.Modules.Users.Features.Register;

using MentorSync.SharedKernel.Abstractions.CQRS;

/// <summary>
/// Represents a user registration command.
/// </summary>
public sealed record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<AuthResponse>;
```

## Required Elements

1. **File-scoped namespace**: `namespace Path.To.Feature;`
2. **Sealed record**: `public sealed record CommandName(...) : ICommand<T>;`
3. **Immutable properties**: All as positional parameters
4. **Implements ICommand<T>**: Generic type = response DTO
5. **XML docs**: `<summary>` explaining the operation
6. **Proper naming**: Action verb + entity (Create, Update, Delete, etc.)

## Commands with Complex Data

For commands with many parameters, use nested types:

```cs
namespace MentorSync.Modules.Materials.Features.CreateMaterial;

using MentorSync.SharedKernel.Abstractions.CQRS;

public sealed record CreateMaterialCommand(
    string Title,
    string Description,
    string Category,
    int DifficultyLevel,
    CreateMaterialCommand.ContentData Content
) : ICommand<MaterialResponse>
{
    /// <summary>
    /// Represents material content with media and structure.
    /// </summary>
    public sealed record ContentData(
        string Text,
        string[]? MediaUrls,
        string? VideoUrl
    );
}
```

## Validation Commands

Commands should NOT contain validation logic. Keep them simple data holders.

Validation belongs in `{Feature}CommandValidator.cs`:

```cs
namespace MentorSync.Modules.Users.Features.Register;

using FluentValidation;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Z]").WithMessage("Password must contain an uppercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain a digit");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must be at most 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must be at most 100 characters");
    }
}
```

## Convention

-   Use positional records (not properties): `record Foo(string Bar, int Baz);`
-   All properties required (no `?` nullable for command data)
-   Name matches feature folder: `CreateUserCommand` in `Features/CreateUser/`
-   Response type is specific DTO: `Result<UserResponse>` not `Result<User>`
-   Never use `class` for commands; always `sealed record`

## Complex Scenarios

Multi-step commands can reuse nested records:

```cs
public sealed record UpdateUserProfileCommand(
    string UserId,
    PersonalInfo Personal,
    ProfessionalInfo Professional
) : ICommand<UserProfileResponse>
{
    public sealed record PersonalInfo(
        string FirstName,
        string LastName,
        string? Bio
    );

    public sealed record ProfessionalInfo(
        string Title,
        string[] Expertise,
        int YearsExperience
    );
}
```

## Naming Conventions

| Operation   | Pattern                   | Example                 |
| ----------- | ------------------------- | ----------------------- |
| Create      | `Create{Entity}Command`   | `CreateMaterialCommand` |
| Update      | `Update{Entity}Command`   | `UpdateUserCommand`     |
| Delete      | `Delete{Entity}Command`   | `DeleteMaterialCommand` |
| Send/Submit | `Send{Action}Command`     | `SendInvitationCommand` |
| Change      | `Change{Property}Command` | `ChangePasswordCommand` |

## File Organization

```
Features/
├── CreateMaterial/
│   ├── CreateMaterialCommand.cs          // Record definition
│   ├── CreateMaterialCommandValidator.cs // Validation rules
│   ├── CreateMaterialCommandHandler.cs   // Business logic
│   ├── CreateMaterialRequest.cs          // HTTP request DTO
│   ├── CreateMaterialResponse.cs         // HTTP response DTO
│   └── CreateMaterialEndpoint.cs         // Route mapping
└── UpdateMaterial/
    └── ...
```

## Testing Example

```cs
[TestClass]
public class CreateMaterialCommandTests
{
    [TestMethod]
    public void Command_CreatesWithValidData()
    {
        // Arrange
        var command = new CreateMaterialCommand(
            Title: "C# Basics",
            Description: "Learn C# fundamentals",
            Category: "Programming",
            DifficultyLevel: 1,
            Content: new(
                Text: "Content here",
                MediaUrls: new[] { "https://example.com/image.jpg" },
                VideoUrl: null
            )
        );

        // Assert - record created successfully
        Assert.AreEqual("C# Basics", command.Title);
        Assert.AreEqual(1, command.DifficultyLevel);
    }
}
```

## Best Practices

-   ✅ Keep commands simple and focused on data
-   ✅ Use clear, action-oriented names
-   ✅ Make all properties immutable (sealed record)
-   ✅ Use nested records for complex structures
-   ✅ Never put business logic in commands
-   ✅ Validate in separate validator class
-   ✅ Include XML documentation for clarity
-   ❌ Never use `null` coalescing or optional logic
-   ❌ Don't create commands with computed properties
-   ❌ Never mix concerns (auth + business logic)

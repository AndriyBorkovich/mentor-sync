# MentorSync GitHub Copilot Instructions

This file enables GitHub Copilot and AI tools to generate features consistent with MentorSync's architecture. Generated via Bitovi ai-enablement-prompts instruction-generation chain.

## Core Technology Stack

### Frontend

-   **React 18+** with TypeScript, Vite, TailwindCSS
-   **React Router v6** for client-side routing
-   **Axios** API client + react-toastify for notifications
-   **react-hook-form** for form state management
-   **Framer Motion** for animations (optional)

### Backend (Modular Monolith Architecture)

-   **C# .NET 9** (C# 13 features), ASP.NET Core WebAPI
-   **Architecture**: Modular Monolith with feature modules in `src/Modules/{Domain}/`
    -   Independent modules: Users, Materials, Scheduling, Ratings, Recommendations, Notifications
    -   Each module has isolated DbContext, Entities, Features, Services
    -   Shared kernel: `MentorSync.SharedKernel` for common abstractions
    -   Clear module boundaries enable future microservices extraction
-   **Custom CQRS**: ICommand/IQuery/IMediator pattern (migrated from MediatR)
-   **Minimal APIs** via IEndpoint interface (no controllers)
-   **Ardalis.Result** for Result<T> pattern
-   **EF Core** ORM with PostgreSQL multi-schema design (one schema per module)
    -   **Direct DbContext calls**: Handlers and services use DbContext directly (no repository pattern)
    -   **LINQ queries**: Leverage EF Core LINQ for data access
-   **FluentValidation** for request validation
-   **JWT authentication** with refresh token support

### Infrastructure

-   .NET Aspire for service orchestration
-   Bicep templates for Azure IaC
-   GitHub Actions for CI/CD
-   PostgreSQL with multi-schema per module design
-   Azure Container Apps for hosting

## General Guidelines

-   Make only high confidence suggestions when reviewing code changes.
-   Always use latest C# and .NET 9 features (C# 13).
-   Never modify global.json unless explicitly asked.
-   Focus on consistency with existing patterns documented in `.results/4-domains/` and `.results/5-style-guides/`.
-   Reference real code examples from the MentorSync codebase when uncertain.
-   **Architecture tests validate** modular monolith constraints - all module dependencies must go through `*.Contracts` projects only
-   **Run architecture tests** before committing: `dotnet test tests/MentorSync.ArchitectureTests/`

## C# Formatting & Code Style

-   Use rules from .editorconfig file

### Namespace & Using

-   File-scoped namespaces: `namespace MentorSync.Domain;` (single-line, semicolon-terminated)
-   Single-line using directives, no multiline blocks
-   Organize usings: System, third-party, then internal

### Code Blocks

-   Newline before opening brace: `if (condition)\n{`
-   Applied to: `if`, `for`, `while`, `foreach`, `using`, `try`, `switch`, `class`, `method`, etc.
-   Final return statement on its own line (not inline)
-   Single-line code blocks are acceptable for simple conditionals: `if (x) return;`

### Pattern Matching & Expressions

-   Use pattern matching instead of traditional casts:
    ```cs
    if (obj is LoginCommand loginCmd) { /* use loginCmd */ }
    if (value is null) { /* handle null */ }
    ```
-   Prefer switch expressions over switch statements:
    ```cs
    var message = status switch
    {
        "success" => "Operation completed",
        "error" => "Operation failed",
        _ => "Unknown"
    };
    ```

### Naming & Constants

-   Use `nameof()` instead of string literals for member references
-   PascalCase: Classes, methods, properties, events
-   camelCase: private fields, local variables, parameters, constants
-   Interfaces prefixed with `I`: `ICommand`, `IQueryHandler`

### XML Documentation

-   Required for all public APIs (methods, properties, classes)
-   Include `<summary>`, `<param>`, `<returns>`, `<exception>` tags
-   Include `<example>` and `<code>` sections for complex APIs:
    ```cs
    /// <summary>
    /// Handles user login with email and password.
    /// </summary>
    /// <param name="command">The login command with email and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>AuthResponse containing JWT tokens.</returns>
    /// <exception cref="InvalidOperationException">Thrown if credentials are invalid.</exception>
    public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
    ```

### Nullable Reference Types

-   **Default non-nullable**: Declare `string email = ""` (not `string? email`)
-   **Validation at entry**: Check for null at API boundaries (controllers, endpoints)
-   **Comparison operators**: Always use `is null` / `is not null`, never `== null` or `!= null`
-   **Trust annotations**: Don't add redundant null checks when type system guarantees non-null
-   **No nullable `?` on reference types**: Project has no `<Nullable>` flag; don't add operator

### Async/Await

-   **All I/O operations are async**: Database, HTTP, file, cache operations
-   **Never use `async void`**: Exception-only for event handlers
-   **ConfigureAwait(false)**: Library code must use to prevent deadlocks
-   **Never block on async**: No `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`
-   **Always accept CancellationToken**: All async methods signature: `CancellationToken cancellationToken = default`
-   **"Async" suffix**: All async methods named `GetDataAsync()`, `SendEmailAsync()`, etc.
-   **Exception handling**: Try-catch in async methods or observe returned Task

### Records & Immutability

-   Use `sealed record` for commands, queries, DTOs:
    ```cs
    public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
    ```
-   Immutable by default; only use `class` for stateful services
-   Records provide structural equality automatically

## Frontend Architecture

### Component Structure & Patterns

**File Organization**:

```
src/features/{domain}/
├── components/        (Feature-specific, not reusable)
├── hooks/            (Domain logic: useUserProfile, useMaterials)
├── pages/            (Page components: UserProfilePage.tsx)
├── services/         (API calls: userService.ts)
└── types/            (TypeScript interfaces)
```

**Naming Conventions**:

-   Files: `PascalCase.tsx` (e.g., `MaterialCard.tsx`, `UserProfilePage.tsx`)
-   Components: Functional components with `React.FC<Props>` interface
-   Hooks: `use` prefix (e.g., `useUserProfile`, `useMaterials`)
-   Services: `{domain}Service.ts` exporting async functions
-   Types/Interfaces: `PascalCase.ts` files (e.g., `User.ts`, `Material.ts`)

**Component Example**:

```tsx
interface MaterialCardProps {
	material: Material;
	onSelect: (id: string) => void;
}

export const MaterialCard: React.FC<MaterialCardProps> = ({
	material,
	onSelect,
}) => {
	const handleClick = useCallback(() => {
		onSelect(material.id);
	}, [material.id, onSelect]);

	return (
		<div
			onClick={handleClick}
			className="border rounded-lg p-4 hover:shadow-md"
		>
			<h3 className="font-bold">{material.title}</h3>
			<p className="text-gray-600">{material.description}</p>
		</div>
	);
};
```

### State Management Hierarchy

1. **Global State (Context)**

    - `AuthContext`: Current user, authentication status, roles
    - `OnboardingContext`: Onboarding progress, steps
    - Used only for truly global concerns
    - Wrapped in providers at App.tsx root

2. **Feature-Scoped State (Custom Hooks)**

    - Domain logic: `useUserProfile`, `useMaterials`, `useChat`, `useScheduling`
    - Data fetching + caching
    - Shared across components in same domain
    - Example:

        ```tsx
        export function useMaterials(filters?: MaterialFilters) {
        	const [data, setData] = useState<Material[]>([]);
        	const [loading, setLoading] = useState(true);
        	const [error, setError] = useState<string | null>(null);

        	useEffect(() => {
        		const fetch = async () => {
        			try {
        				const result = await materialService.list(filters);
        				setData(result);
        			} catch (err) {
        				setError("Failed to load materials");
        			} finally {
        				setLoading(false);
        			}
        		};
        		fetch();
        	}, [filters]);

        	return { data, loading, error };
        }
        ```

3. **Local Component State (useState)**
    - UI-only: Modal open/closed, form inputs, dropdowns, pagination state
    - Do NOT share across components
    - Example:
        ```tsx
        const [isOpen, setIsOpen] = useState(false);
        const [sortBy, setSortBy] = useState("relevance");
        ```

### Memoization Strategy

**useMemo**: For expensive computations

```tsx
const filteredMaterials = useMemo(() => {
	return data.filter(
		(m) => m.category === filters.category && m.level >= filters.minLevel
	);
}, [data, filters.category, filters.minLevel]);
```

**useCallback**: For event handlers passed to children

```tsx
const handleFilterChange = useCallback((newFilter: MaterialFilter) => {
	setFilters(newFilter);
}, []);
```

**Critical**: Separate unrelated state to prevent unnecessary re-renders

```tsx
// ❌ WRONG: Filter changes trigger pagination reset
const [combined, setCombined] = useState({
	filters: {},
	pagination: { page: 1 },
});

// ✅ CORRECT: Independent state objects
const [filters, setFilters] = useState({});
const [pagination, setPagination] = useState({ page: 1 });
```

### Routing

**Protected Routes**:

```tsx
<ProtectedRoute>
	<Dashboard />
</ProtectedRoute>
```

**Role-Based Routes**:

```tsx
<RoleBasedRoute allowedRoles={["Mentor"]}>
	<MentorDashboard />
</RoleBasedRoute>
```

**Route Structure**:

-   `/` - Landing
-   `/login` - Public
-   `/register` - Public
-   `/onboarding` - Protected, after login
-   `/dashboard` - Protected
-   `/profile` - Protected
-   `/mentors` - Protected
-   `/materials` - Protected

### Styling with TailwindCSS

-   **Only TailwindCSS utility classes**: No CSS files, no styled-components
-   **Responsive design**: Mobile-first with `sm:`, `md:`, `lg:` prefixes
-   **Color scheme**: Use project color palette (check `tailwind.config.ts`)
-   **No inline styles**: Only `className` with Tailwind utilities
-   **Dark mode**: Supported via `dark:` prefix if enabled

Example:

```tsx
<div className="min-h-screen flex bg-gradient-to-r from-blue-50 to-indigo-100 dark:from-gray-900 dark:to-gray-800">
	<aside className="w-72 bg-white dark:bg-gray-800 shadow-lg p-4">
		{/* Sidebar */}
	</aside>
	<main className="flex-1 p-8">{/* Main content */}</main>
</div>
```

### API Client & Data Layer

**Centralized Axios Instance** (`src/shared/services/api.ts`):

```ts
export const apiClient = axios.create({
	baseURL: process.env.REACT_APP_API_URL || "http://localhost:5001/api",
	timeout: 30000,
});

// Request interceptor: Auto-inject Bearer token
apiClient.interceptors.request.use((config) => {
	const token = localStorage.getItem("accessToken");
	if (token) {
		config.headers.Authorization = `Bearer ${token}`;
	}
	return config;
});

// Response interceptor: Handle errors
apiClient.interceptors.response.use(
	(response) => response,
	(error) => {
		if (error.response?.status === 401) {
			localStorage.removeItem("accessToken");
			window.location.href = "/login";
		}
		toast.error(error.response?.data?.detail || "An error occurred");
		return Promise.reject(error);
	}
);
```

**Domain Services** (`src/features/{domain}/services/{domain}Service.ts`):

```ts
export const materialService = {
	list: async (filters?: MaterialFilters) => {
		const response = await apiClient.get("/materials", { params: filters });
		return response.data;
	},

	getById: async (id: string) => {
		const response = await apiClient.get(`/materials/${id}`);
		return response.data;
	},

	create: async (data: CreateMaterialRequest) => {
		const response = await apiClient.post("/materials", data);
		return response.data;
	},
};
```

### Error Handling (Frontend)

```tsx
const [error, setError] = useState<string | null>(null);

useEffect(() => {
	const fetch = async () => {
		try {
			const data = await fetchUserProfile();
			setData(data);
			setError(null);
		} catch (err) {
			// User-friendly message
			toast.error("Failed to load profile");
			// Technical logging
			console.error("Profile fetch error:", err);
			setError("Unable to load your profile. Please try again.");
		}
	};
	fetch();
}, []);
```

**Error Display**:

-   Use `react-toastify` for temporary notifications
-   Show error messages on component (for forms, lists)
-   Log technical errors to console for debugging

## Backend Architecture

### Custom CQRS Pattern

**Command** (Write Operation):

```cs
public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
```

**CommandHandler**:

```cs
public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly UsersDbContext _dbContext;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(UsersDbContext dbContext, IJwtService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(command.Email))
        {
            return Result.Invalid(new ValidationError(nameof(command.Email), "Email is required"));
        }

        // Business logic - Direct DbContext query
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);
        if (user is null)
        {
            return Result.Unauthorized("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
        {
            return Result.Unauthorized("Invalid email or password");
        }

        // Success
        var tokens = _jwtService.GenerateTokens(user);
        return Result.Success(new AuthResponse(tokens.AccessToken, tokens.RefreshToken));
    }
}
```

**Query** (Read Operation):

```cs
public sealed record GetUserProfileQuery(string UserId) : IQuery<UserProfileResponse>;
```

**QueryHandler**:

```cs
public sealed class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    private readonly UsersDbContext _dbContext;

    public GetUserProfileQueryHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserProfileResponse>> Handle(
        GetUserProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        // Direct DbContext query
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        if (user is null)
        {
            return Result.NotFound($"User '{query.UserId}' not found");
        }

        var response = UserProfileResponse.FromUser(user);
        return Result.Success(response);
    }
}
```

**Endpoint** (Registration):

```cs
public sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", Handle)
            .WithName("Login")
            .WithOpenApi()
            .AllowAnonymous();
    }

    private static async Task<IResult> Handle(
        LoginRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result);
    }
}
```

### Minimal APIs & IEndpoint Pattern

-   All routes defined via `IEndpoint` interface implementations
-   No controller classes anywhere in codebase
-   Endpoints registered in `Program.cs`:
    ```cs
    services.RegisterEndpoints();
    ```
-   Endpoint registration helper in ServiceDefaults:

    ```cs
    public static IServiceCollection RegisterEndpoints(this IServiceCollection services)
    {
        var endpointType = typeof(IEndpoint);
        var assembly = typeof(Program).Assembly;
        var endpointTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var type in endpointTypes)
        {
            var endpoint = Activator.CreateInstance(type) as IEndpoint;
            endpoint?.MapEndpoint(app);
        }

        return services;
    }
    ```

### Result<T> Pattern

All operations return `Result<T>` from Ardalis.Result:

```cs
// Success
Result.Success(value)

// Error (general)
Result.Error("Something went wrong")

// Not Found
Result.NotFound("Resource not found")

// Unauthorized
Result.Unauthorized("Authentication required")

// Forbidden
Result.Forbidden("Access denied")

// Invalid (validation)
Result.Invalid(new ValidationError("fieldName", "error message"))

// Check result
if (result.IsSuccess)
{
    var data = result.Value;
}
else
{
    var errors = result.Errors; // IEnumerable<string>
}
```

### Pagination

**Request**:

```cs
public sealed record GetMaterialsQuery(
    int PageNumber = 1,
    int PageSize = 12,
    string? SearchTerm = null,
    string? Category = null
) : IQuery<PagedResponse<MaterialResponse>>;
```

**Response**:

```cs
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

**Handler**:

```cs
public async Task<Result<PagedResponse<MaterialResponse>>> Handle(
    GetMaterialsQuery query,
    CancellationToken cancellationToken = default)
{
    var materials = _dbContext.Materials.AsQueryable();

    if (!string.IsNullOrWhiteSpace(query.SearchTerm))
    {
        materials = materials.Where(m => m.Title.Contains(query.SearchTerm));
    }

    if (!string.IsNullOrWhiteSpace(query.Category))
    {
        materials = materials.Where(m => m.Category == query.Category);
    }

    var totalCount = await materials.CountAsync(cancellationToken);
    var items = await materials
        .Skip((query.PageNumber - 1) * query.PageSize)
        .Take(query.PageSize)
        .OrderByDescending(m => m.CreatedAt)
        .Select(m => MaterialResponse.FromMaterial(m))
        .ToListAsync(cancellationToken);

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

### Validation

Use FluentValidation for request validation:

```cs
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}
```

### Error Handling (Backend)

**Global Exception Handler**:

```cs
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred");

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
```

**RFC 7807 Problem Details Format**:

```json
{
	"type": "https://example.com/probs/invalid-request",
	"title": "Invalid Request",
	"status": 400,
	"detail": "The email field is required",
	"instance": "/api/users/login"
}
```

## File Organization Reference

### Frontend Structure

```
src/MentorSync.UI/
├── src/
│   ├── features/
│   │   ├── auth/
│   │   │   ├── pages/
│   │   │   │   ├── LoginPage.tsx
│   │   │   │   └── RegisterPage.tsx
│   │   │   ├── components/
│   │   │   │   └── LoginForm.tsx
│   │   │   ├── services/
│   │   │   │   └── authService.ts
│   │   │   ├── contexts/
│   │   │   │   └── AuthContext.tsx
│   │   │   └── hooks/
│   │   │       └── usePersistentAuth.ts
│   │   ├── materials/
│   │   │   ├── pages/
│   │   │   │   ├── MaterialsPage.tsx
│   │   │   │   └── MaterialDetailPage.tsx
│   │   │   ├── components/
│   │   │   │   ├── MaterialCard.tsx
│   │   │   │   ├── MaterialFilter.tsx
│   │   │   │   └── MaterialForm.tsx
│   │   │   ├── services/
│   │   │   │   └── materialService.ts
│   │   │   ├── hooks/
│   │   │   │   └── useMaterials.ts
│   │   │   └── types/
│   │   │       └── Material.ts
│   │   └── scheduling/
│   │       └── ...
│   ├── components/
│   │   ├── ui/
│   │   │   ├── Button.tsx
│   │   │   ├── Input.tsx
│   │   │   ├── Modal.tsx
│   │   │   └── ...
│   │   └── layout/
│   │       ├── Sidebar.tsx
│   │       ├── Header.tsx
│   │       ├── Footer.tsx
│   │       └── Navbar.tsx
│   ├── shared/
│   │   ├── services/
│   │   │   └── api.ts
│   │   ├── contexts/
│   │   │   └── NotificationsContext.tsx
│   │   ├── hooks/
│   │   │   └── useToast.ts
│   │   └── types/
│   │       └── Common.ts
│   ├── App.tsx
│   ├── routes.tsx
│   └── main.tsx
├── package.json
├── tsconfig.json
├── vite.config.ts
└── tailwind.config.ts
```

### Backend Structure

```
src/Modules/{Domain}/
├── Features/
│   ├── {Feature}/
│   │   ├── {Feature}Command.cs
│   │   ├── {Feature}CommandHandler.cs
│   │   ├── {Feature}CommandValidator.cs
│   │   ├── {Feature}Endpoint.cs
│   │   ├── {Feature}Request.cs
│   │   └── {Feature}Response.cs
│   ├── Get{Entity}/
│   │   ├── Get{Entity}Query.cs
│   │   ├── Get{Entity}QueryHandler.cs
│   │   ├── Get{Entity}Response.cs
│   │   └── Get{Entity}Endpoint.cs
│   └── ...
├── Data/
│   ├── {Domain}DbContext.cs
│   └── Configurations/
│       ├── {Entity}Configuration.cs
│       └── ...
├── Domain/
│   ├── {Entity}.cs
│   ├── {ValueObject}.cs
│   └── Events/
│       └── {Entity}{Event}.cs
├── Infrastructure/
│   └── Services/
│       └── {Service}.cs
├── Registration.cs
└── {Domain}Module.cs
```

## Modular Monolith Architecture

### Inter-Module Communication

-   Modules communicate **ONLY** through `*.Contracts` projects
-   Never reference another module's implementation (Data, Domain, Features, Infrastructure)
-   All cross-module types must be defined in `{Module}.Contracts` namespace
-   Example: `MentorSync.Users` can only depend on `MentorSync.Materials.Contracts`, NOT `MentorSync.Materials`
-   **Validated by architecture tests** - violations cause test failures

### Module Structure

MentorSync uses a **modular monolith** approach with independent feature modules in `src/Modules/{Domain}/`. Each module contains:

-   **Features/** - CQRS operations (Commands, Queries, Handlers, Endpoints, Validators, DTOs)
-   **Data/** - Module-specific DbContext and EF Core configurations
-   **Domain/** - Domain entities and value objects
-   **Infrastructure/** - Services and utilities
-   **ModuleRegistration.cs** - Dependency injection setup for the module
-   **{Domain}Contracts/** - Public API exposed to other modules

### Core Modules

| Module              | Responsibility                        | Schema            |
| ------------------- | ------------------------------------- | ----------------- |
| **Users**           | Authentication, user profiles, skills | `users`           |
| **Materials**       | Learning materials, tags, attachments | `materials`       |
| **Scheduling**      | Session booking, availability         | `scheduling`      |
| **Ratings**         | Reviews and mentor ratings            | `ratings`         |
| **Recommendations** | ML-driven suggestions                 | `recommendations` |
| **Notifications**   | Email and notification delivery       | `notifications`   |

### Module Benefits

-   **Independent Development**: Teams work on separate modules
-   **Clear Boundaries**: Minimal coupling between modules
-   **Isolated Databases**: Each module has its own PostgreSQL schema
-   **Scalability Path**: Can migrate individual modules to microservices
-   **Testability**: Modules tested independently
-   **Maintainability**: Related features grouped together

### Adding New Features to Modules

When adding a feature:

1. Create a `Features/{Feature}/` folder in the module
2. Add Command/Query, Handler, Validator, Endpoint, Request, Response
3. Register in module's **ModuleRegistration.cs**
4. Endpoint auto-discovered via IEndpoint interface
5. Module auto-registered in `Program.cs`

### Inter-Module Communication

-   Modules communicate via **service interfaces** from Contracts projects
-   **Event publishing** for cross-module notifications
-   **Database foreign keys** for related entities
-   Future: Direct HTTP calls when modules become microservices

## Common Development Tasks

### Add a New Page

1. Create page file: `src/features/{domain}/pages/{PageName}Page.tsx`
2. Include layout: Sidebar + Header + main content
3. Check authentication: Use `useAuth()` hook or `<ProtectedRoute />`
4. Fetch data with custom hook
5. Handle loading/error states
6. Add route in `src/routes.tsx`

Example:

```tsx
export const UserProfilePage: React.FC = () => {
	const { isAuthenticated, user } = useAuth();
	const { data: profile, loading, error } = useUserProfile(user?.id);

	if (!isAuthenticated) return <Navigate to="/login" />;
	if (loading) return <LoadingSpinner />;
	if (error) return <ErrorMessage message={error} />;

	return (
		<div className="min-h-screen flex">
			<Sidebar />
			<div className="flex-1 flex flex-col">
				<Header />
				<main className="flex-1 p-8">
					{profile && <ProfileForm profile={profile} />}
				</main>
			</div>
		</div>
	);
};
```

### Add a New CQRS Feature (Backend)

1. Create command/handler files in `Features/{Feature}/`
2. Implement `ICommand<T>` and `ICommandHandler<T, U>`
3. Add validation with FluentValidation
4. Create endpoint implementing `IEndpoint`
5. Register in module's `Registration.cs`
6. Test with curl or API client

### Add a New API Service (Frontend)

1. Create service file: `src/features/{domain}/services/{domain}Service.ts`
2. Export async functions calling `apiClient`
3. Create corresponding hook: `use{Feature}()`
4. Implement standard pattern: `[data, loading, error]` state
5. Export hook from hook file or `index.ts`

### Implement Pagination

1. Frontend: Add `pagination` state independent from `filters`
2. Backend: Implement query with `PageNumber` and `PageSize` parameters
3. Response includes `totalCount`, `hasNextPage`, `hasPreviousPage`
4. UI: Use pagination controls to update state
5. Never combine filter and pagination changes in single state

### Add Authentication to Endpoint

1. Add `[Authorize]` attribute or check user in handler
2. Use `httpContext.User.FindFirst("sub")?.Value` for user ID
3. Return `Unauthorized()` if not authenticated
4. Check roles: `httpContext.User.IsInRole("Mentor")`

## Integration Rules

### State Management Integration

-   **Global concerns** → AuthContext, OnboardingContext
-   **Feature data** → Custom hooks (useUserProfile, useMaterials)
-   **UI state** → Component useState
-   **DO NOT** create new contexts for feature data
-   **DO NOT** use Redux or other state libraries

### API Integration Pattern

1. Define service function in domain service file
2. Create hook wrapping the service call
3. Use hook in component
4. Handle loading/error/success states
5. Never make API calls directly in components

### Backend Integration Pattern

1. Command/Query record definition
2. Validator with FluentValidation
3. Handler implementation (ICommandHandler or IQueryHandler)
    - Inject DbContext directly (no repositories)
    - Use LINQ queries directly on DbContext
    - All database operations via EF Core
4. Endpoint mapping via IEndpoint interface
5. Register in module's Registration.cs

### Integration Rules

### State Management Integration

-   **Global concerns** → AuthContext, OnboardingContext
-   **Feature data** → Custom hooks (useUserProfile, useMaterials)
-   **UI state** → Component useState
-   **DO NOT** create new contexts for feature data
-   **DO NOT** use Redux or other state libraries

### API Integration Pattern

1. Define service function in domain service file
2. Create hook wrapping the service call
3. Use hook in component
4. Handle loading/error/success states
5. Never make API calls directly in components

### Backend Integration Pattern

1. Command/Query record definition
2. Validator with FluentValidation
3. Handler implementation (ICommandHandler or IQueryHandler)
    - Inject DbContext directly (no repositories)
    - Use LINQ queries directly on DbContext
    - All database operations via EF Core
4. Endpoint mapping via IEndpoint interface
5. Register in module's Registration.cs

### Error Flow

1. API client interceptor catches all errors
2. Toast notification shows user-friendly message
3. Technical error logged to console
4. Component state may show error message for retryability
5. 401 response triggers logout automatically

### Architecture Constraints (Enforced by Tests)

**Module Dependencies:**

-   Modules can ONLY depend on `*.Contracts` from other modules
-   Direct dependencies on Data, Domain, Features, Infrastructure are forbidden
-   All inter-module types must be in `*.Contracts` namespace

**CQRS Pattern:**

-   Commands and Queries must follow naming conventions (end with Command/Query)
-   Handlers must end with CommandHandler/QueryHandler
-   All must reside in Features namespace

**Layer Encapsulation:**

-   Only Endpoints and Contracts should be public
-   Data and Domain layers are internal
-   Features are public entry points for module functionality

## Performance Best Practices

-   Lazy load routes: `React.lazy(() => import('...'))`
-   Memoize expensive computations: `useMemo`
-   Memoize callbacks passed to children: `useCallback`
-   Paginate large lists (max 50 items per page)
-   Cache API responses in custom hooks
-   Use code splitting for large features
-   Separate filter and pagination state (no cascade)
-   Use **AsNoTracking()** for read-only queries in DbContext
-   Project to DTOs with **Select()** instead of loading full entities

## Key Differences from Standard Projects

-   **Modular Monolith** architecture (not traditional monolith or microservices)
-   **Custom CQRS** instead of MediatR (lightweight, explicit)
-   **Minimal APIs** instead of controllers (less boilerplate)
-   **Result<T> pattern** for all operations (explicit success/error)
-   **Context API only** for state (no Redux, no external state libraries)
-   **TailwindCSS only** (no CSS files per component)
-   **Direct DbContext calls** in handlers (no repository pattern)
-   **LINQ queries** directly on DbContext (no abstraction layers)
-   **Contracts-first inter-module communication** (no direct module dependencies)
-   **Architecture tests** validate all constraints automatically

## When You're Stuck

1. Check `.results/4-domains/` for similar patterns
2. Search existing codebase for similar features
3. Review relevant `.results/5-style-guides/*.md` file
4. Look at real implementations in `src/` or `Modules/`
5. Consult `.editorconfig` for formatting questions
6. Reference this guide for architecture questions
7. Run architecture tests: `dotnet test tests/MentorSync.ArchitectureTests/` to validate changes

# MentorSync Build Instructions

This document synthesizes all instruction-generation outputs and provides comprehensive guidance for building features consistently with MentorSync architecture.

## Instruction-Generation Materials

All outputs are organized by step:

1. **TechStack/overview.md** - Complete technology stack analysis
2. **CodebaseStructure/file-categorization.json** - 22 file categories with 130+ files cataloged
3. **Architecture/domains.json** - 13 architectural domains with patterns
4. **Guides/DomainDeepDives/\*.md** - Deep-dive guides (7 files, 2000+ lines) with real code examples:
    - frontend-ui.md, auth-flow.md, state-management.md
    - backend-cqrs.md, data-access.md, error-handling.md, performance-optimization.md
5. **Guides/CodingStandards/\*.md** - Per-category coding conventions (11 files):
    - react-pages.md, custom-hooks.md, feature-components.md, ui-components.md
    - react-contexts.md, csharp-commands.md, csharp-queries.md
    - csharp-handlers.md, layout-components.md, frontend-services.md, csharp-endpoints.md

## File Categories Reference

| Category                    | Location                                    | Pattern                     | Count |
| --------------------------- | ------------------------------------------- | --------------------------- | ----- |
| **Pages**                   | `src/features/{domain}/pages/`              | `{Name}Page.tsx`            | 16    |
| **Feature Components**      | `src/features/{domain}/components/`         | `{Name}.tsx`                | 35+   |
| **UI Components**           | `src/components/ui/`                        | `{Name}.tsx`                | 8     |
| **Layout Components**       | `src/components/layout/`                    | `{Name}.tsx`                | 5     |
| **Hooks**                   | `src/features/{domain}/hooks/`              | `use{Name}.ts`              | 7     |
| **Contexts**                | `src/features/{domain}/contexts/`           | `{Name}Context.tsx`         | 3     |
| **Services**                | `src/features/{domain}/services/`           | `{domain}Service.ts`        | 10    |
| **Types**                   | `src/features/{domain}/types/`              | `{Name}.ts`                 | 9     |
| **Endpoints**               | `src/Modules/{Domain}/Features/`            | `{Action}Endpoint.cs`       | 17    |
| **Commands**                | `src/Modules/{Domain}/Features/`            | `{Action}Command.cs`        | 13    |
| **Command Handlers**        | `src/Modules/{Domain}/Features/`            | `{Action}CommandHandler.cs` | 13    |
| **Queries**                 | `src/Modules/{Domain}/Features/`            | `{Action}Query.cs`          | 4     |
| **Query Handlers**          | `src/Modules/{Domain}/Features/`            | `{Action}QueryHandler.cs`   | 4     |
| **Entities**                | `src/Modules/{Domain}/Domain/`              | `{Name}.cs`                 | 13    |
| **DTOs/Responses**          | `src/Modules/{Domain}/Features/`            | `{Name}Response.cs`         | 20+   |
| **Validators**              | `src/Modules/{Domain}/Features/`            | `{Action}Validator.cs`      | 10+   |
| **DbContexts**              | `src/Modules/{Domain}/Data/`                | `{Domain}DbContext.cs`      | 5     |
| **Database Configurations** | `src/Modules/{Domain}/Data/Configurations/` | `{Entity}Configuration.cs`  | 15+   |
| **Services**                | `src/Modules/{Domain}/Infrastructure/`      | `{Name}Service.cs`          | 6     |
| **Bicep Templates**         | `aspire/bicep-templates/`                   | `{resource}.module.bicep`   | 4     |
| **Documentation**           | `docs/`                                     | `{Topic}.md`                | 12    |

## Feature Development Workflow

### Adding a New Frontend Feature

```
1. Create feature folder structure:
   src/features/{feature}/
   ├── pages/{FeatureName}Page.tsx
   ├── components/{Components}.tsx
   ├── hooks/use{Feature}.ts
   ├── services/{feature}Service.ts
   └── types/{Types}.ts

2. Define types:
   - Create src/features/{feature}/types/{Name}.ts
   - Export interfaces for all component props

3. Create custom hook:
   - File: src/features/{feature}/hooks/use{Feature}.ts
   - Pattern: return { data, loading, error }
   - All API calls via service layer

4. Create service functions:
   - File: src/features/{feature}/services/{feature}Service.ts
   - Export async functions calling apiClient
   - All typed with request/response interfaces

5. Create page component:
   - File: src/features/{feature}/pages/{Feature}Page.tsx
   - Include Sidebar + Header layout
   - Check authentication with useAuth()
   - Fetch data with custom hook
   - Show loading/error states

6. Create feature components:
   - Files: src/features/{feature}/components/{Component}.tsx
   - Take data and callbacks as props (no direct API calls)
   - All styling via TailwindCSS
   - Memoize handlers with useCallback

7. Add to routes:
   - Update src/routes.tsx
   - Wrap with <ProtectedRoute /> or <RoleBasedRoute />
   - Use React.lazy() for code splitting

8. Register endpoint (backend):
   - Create Command/Query in Features/
   - Create Handler
   - Create Endpoint (IEndpoint)
   - Add validation with FluentValidation
   - Register in module's Registration.cs
```

### Adding a New Backend API

```
1. Create module structure:
   src/Modules/{Domain}/Features/{Feature}/
   ├── {Feature}Command.cs (or Query.cs)
   ├── {Feature}CommandHandler.cs (or QueryHandler.cs)
   ├── {Feature}CommandValidator.cs
   ├── {Feature}Endpoint.cs
   ├── {Feature}Request.cs
   └── {Feature}Response.cs

2. Define Command/Query:
   - sealed record inheriting ICommand<T> or IQuery<T>
   - Immutable positional parameters
   - XML documentation required

3. Add validation:
   - Class inheriting AbstractValidator<CommandType>
   - FluentValidation rules

4. Implement handler:
   - ICommandHandler<T, U> or IQueryHandler<T, U>
   - Inject DbContext directly (no repositories)
   - Query DbContext with LINQ (e.g., `_dbContext.Users.Where(...).ToListAsync()`)
   - Business logic in Handle() method
   - Return Result<T> from Ardalis.Result
   - Use `AsNoTracking()` for read-only queries

5. Create endpoint:
   - Class implementing IEndpoint
   - MapEndpoint() method registers route
   - Handle() method maps request → command/query → response
   - Include OpenAPI documentation

6. Define DTOs:
   - Request: Matches HTTP request body
   - Response: Matches HTTP response body
   - Never use entities directly

7. Register in module:
   - Add to {Domain}Module.cs
   - Register handler in DI container
   - Register DbContext if needed
   - Register validator if needed
   - Endpoint auto-discovered via IEndpoint interface
   - Endpoint auto-registered via IEndpoint interface

8. Test:
   - Unit test handler logic
   - Integration test endpoint
   - Test error scenarios
```

## Architecture Integration Rules

### Frontend Integration

**Never mix concerns**:

-   ❌ API calls directly in components (use services)
-   ❌ Feature hooks in other features (use props)
-   ❌ Redux, MobX, Zustand (use Context API + custom hooks)
-   ❌ CSS files (use TailwindCSS only)

**Always follow patterns**:

-   ✅ Separate state into independent concerns (filters ≠ pagination)
-   ✅ Memoize expensive computations (useMemo)
-   ✅ Memoize callbacks passed to children (useCallback)
-   ✅ Use custom hooks for data fetching
-   ✅ Keep components presentation-focused

### Backend Integration

**Never mix concerns**:

-   ❌ Business logic in endpoints (use handlers)
-   ❌ Direct database calls scattered throughout codebase (use DbContext in handlers)
-   ❌ Entities in responses (use DTOs)
-   ❌ Controller classes (use IEndpoint)
-   ❌ Throw exceptions to client (return Result<T>)
-   ❌ Repository pattern (use DbContext directly)

**Always follow patterns**:

-   ✅ Commands for writes, Queries for reads
-   ✅ Handlers contain business logic with direct DbContext access
-   ✅ DbContext accessed directly via LINQ queries
-   ✅ Endpoints map HTTP to commands/queries
-   ✅ All operations return Result<T>
-   ✅ Use `AsNoTracking()` for read-only queries
-   ✅ Project to DTOs with `Select()` instead of loading full entities

### Error Handling Integration

**Frontend**:

1. API client interceptor catches errors
2. User-friendly toast notification shown
3. Technical error logged to console
4. Component error state for retry
5. 401 response triggers automatic logout

**Backend**:

1. Global exception handler catches all exceptions
2. RFC 7807 Problem Details formatted
3. Logged with correlation ID
4. Status code indicates error type (400, 401, 403, 404, 500)

## Example: Adding "Rate Mentor" Feature

### Backend Implementation

```cs
// Step 1: Define command (Modules/Ratings/Features/RateMentor/RateMentorCommand.cs)
public sealed record RateMentorCommand(
    string MentorId,
    string ClientId,
    int Rating,
    string Comment
) : ICommand<RatingResponse>;

// Step 2: Add validator
public sealed class RateMentorCommandValidator : AbstractValidator<RateMentorCommand>
{
    public RateMentorCommandValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Comment).MaximumLength(500);
    }
}

// Step 3: Implement handler (RateMentorCommandHandler.cs)
public sealed class RateMentorCommandHandler(
    RatingsDbContext dbContext)
    : ICommandHandler<RateMentorCommand, RatingResponse>
{
    public async Task<Result<RatingResponse>> Handle(
        RateMentorCommand command,
        CancellationToken ct = default)
    {
        // Direct DbContext query - validate mentor exists
        var mentor = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == command.MentorId, ct);
        if (mentor is null)
            return Result.NotFound("Mentor not found");

        // Create rating entity
        var rating = new Rating(
            command.MentorId,
            command.ClientId,
            command.Rating,
            command.Comment);

        // Add and save
        dbContext.Ratings.Add(rating);
        await dbContext.SaveChangesAsync(ct);

        return Result.Success(RatingResponse.FromRating(rating));
    }
}

// Step 4: Create endpoint (RateMentorEndpoint.cs)
public sealed class RateMentorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ratings", Handle)
            .WithName("RateMentor")
            .RequireAuthorization()
            .Produces<RatingResponse>(200)
            .Produces(404);
    }

    private static async Task<IResult> Handle(
        RateMentorRequest request,
        HttpContext ctx,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new RateMentorCommand(request.MentorId, ctx.User.FindFirst("sub")!.Value, request.Rating, request.Comment);
        var result = await mediator.SendCommandAsync<RateMentorCommand, RatingResponse>(command, ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }
}
```

### Frontend Implementation

```tsx
// Step 1: Create service (features/ratings/services/ratingService.ts)
export const ratingService = {
	createRating: async (mentorId: string, rating: number, comment: string) => {
		const response = await apiClient.post<RatingResponse>("/ratings", {
			mentorId,
			rating,
			comment,
		});
		return response.data;
	},
};

// Step 2: Create hook (features/ratings/hooks/useRatingForm.ts)
export function useRatingForm(mentorId: string) {
	const [rating, setRating] = useState(0);
	const [comment, setComment] = useState("");
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	const submitRating = useCallback(async () => {
		setLoading(true);
		try {
			await ratingService.createRating(mentorId, rating, comment);
			toast.success("Rating submitted!");
			setRating(0);
			setComment("");
		} catch (err) {
			setError("Failed to submit rating");
		} finally {
			setLoading(false);
		}
	}, [mentorId, rating, comment]);

	return {
		rating,
		setRating,
		comment,
		setComment,
		loading,
		error,
		submitRating,
	};
}

// Step 3: Create component (features/ratings/components/RatingForm.tsx)
export const RatingForm: React.FC<{ mentorId: string }> = ({ mentorId }) => {
	const { rating, setRating, comment, setComment, loading, submitRating } =
		useRatingForm(mentorId);

	return (
		<form>
			<div className="mb-4">
				<label>Rating (1-5)</label>
				<div className="flex gap-2">
					{[1, 2, 3, 4, 5].map((i) => (
						<button
							key={i}
							onClick={() => setRating(i)}
							className={rating >= i ? "text-yellow-500" : ""}
						>
							⭐
						</button>
					))}
				</div>
			</div>
			<div className="mb-4">
				<label>Comment</label>
				<textarea
					value={comment}
					onChange={(e) => setComment(e.target.value)}
					maxLength={500}
				/>
			</div>
			<Button onClick={submitRating} isLoading={loading}>
				Submit Rating
			</Button>
		</form>
	);
};

// Step 4: Add to page and route
// Update features/mentors/pages/MentorDetailPage.tsx to include <RatingForm />
// Update routes.tsx with <ProtectedRoute /> wrapper
```

## Performance Checklist

-   ☑️ Separate unrelated state (filters ≠ pagination)
-   ☑️ Memoize expensive computations (useMemo)
-   ☑️ Memoize callbacks passed to children (useCallback)
-   ☑️ Use AsNoTracking() for read-only queries
-   ☑️ Project to DTOs (not full entities)
-   ☑️ Paginate large lists (max 50 items per page)
-   ☑️ Lazy load routes (React.lazy)
-   ☑️ Code split large features
-   ☑️ Cache API responses in hooks
-   ☑️ Use Select projection in queries

## Testing Strategy

-   **Unit Tests**: Handlers, hooks, services, components in isolation
-   **Integration Tests**: Endpoint → handler → DbContext flow
-   **Component Tests**: Rendering, user interactions, error states
-   **API Tests**: Response format, pagination, error codes

## Deployment Checklist

-   ☑️ Environment variables configured
-   ☑️ Database migrations applied
-   ☑️ Frontend built with Vite
-   ☑️ Backend images pushed to registry
-   ☑️ Bicep templates validated
-   ☑️ Health checks configured
-   ☑️ Logging/monitoring setup
-   ☑️ Secrets managed in Azure Key Vault

## Common Mistakes

-   ❌ Creating contexts for feature data (use custom hooks)
-   ❌ API calls directly in components (use services)
-   ❌ Combining unrelated state (separate concerns)
-   ❌ Throwing exceptions in handlers (return Result<T>)
-   ❌ Business logic in endpoints (use handlers)
-   ❌ Using repository pattern (use DbContext directly)
-   ❌ Class components (use functional + hooks)
-   ❌ CSS files (use TailwindCSS)
-   ❌ MediatR handlers (migrate to custom CQRS)
-   ❌ Complex repository abstractions (keep it simple with DbContext)

## Resources

-   **Tech Stack**: `docs/TechStack/overview.md`
-   **File Catalog**: `docs/CodebaseStructure/file-categorization.json`
-   **Architectural Domains**: `docs/Architecture/domains.json`
-   **Domain Deep-Dives**: `docs/Guides/DomainDeepDives/*.md`
-   **Coding Standards**: `docs/Guides/CodingStandards/*.md`
-   **Main Instructions**: `/.github/copilot-instructions.md`

# Error Handling Domain Deep Dive

## Overview

Error handling uses global exception handler middleware on backend with RFC 7807 Problem Details format, and client-side toast notifications with user-friendly messages.

## Backend Global Exception Handler

### GlobalExceptionHandler Implementation

```cs
// src/MentorSync.API/GlobalExceptionHandler.cs
namespace MentorSync.API;

using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ValidationException fluentException:
                {
                    problemDetails.Title = "One or more validation errors occurred.";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var validationErrors = fluentException.Errors
                        .Select(error => error.ErrorMessage)
                        .ToList();
                    problemDetails.Extensions.Add("errors", validationErrors);
                    break;
                }
            case OperationCanceledException:
                problemDetails.Title = "Operation was cancelled";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                break;
            default:
                problemDetails.Title = exception.Message;
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        problemDetails.Status = httpContext.Response.StatusCode;

        // Add trace ID for support references
        problemDetails.Extensions.Add("trace-id", httpContext.TraceIdentifier);
        problemDetails.Extensions.Add("instance", $"{httpContext.Request.Method} {httpContext.Request.Path}");

        logger.LogError(exception, "{ProblemDetailsTitle}", problemDetails.Title);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken)
            .ConfigureAwait(false);

        return true;
    }
}
```

### Exception Handler Configuration

```cs
// src/MentorSync.API/Extensions/ServiceCollectionExtensions.cs
public static void AddExceptionHandling(this IServiceCollection services)
{
    services.AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
            ctx.ProblemDetails.Extensions.Add("instance",
                $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        }
    );

    services.AddExceptionHandler<GlobalExceptionHandler>();
}
```

## RFC 7807 Problem Details Format

### Problem Details Response

```json
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "One or more validation errors occurred.",
	"status": 400,
	"detail": "Email is required",
	"instance": "POST /users/register",
	"trace-id": "0HQ7JOKHOB3IP:00000001",
	"errors": ["Email is required", "Password must be at least 8 characters"]
}
```

### Server Error Response

```json
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
	"title": "Database connection failed",
	"status": 500,
	"instance": "GET /materials/123",
	"trace-id": "0HQ7JOKHOB3IP:00000002"
}
```

## Client-Side Error Handling

### API Response Interceptor

```ts
// src/MentorSync.UI/src/shared/services/api.ts
api.interceptors.response.use(
	(response: AxiosResponse) => response,
	(error: AxiosError) => {
		if (error.response) {
			const apiError = error.response.data as ApiErrorResponse;
			const errorMessage =
				apiError.detail ||
				apiError.title ||
				"Виникла помилка при виконанні запиту";

			console.error("API Error:", {
				status: apiError.status || error.response.status,
				title: apiError.title || "API Error",
				detail: apiError.detail || error.message,
				traceId: apiError.extensions?.["trace-id"],
			});

			// Handle specific status codes
			if (error.response.status === 401) {
				console.warn("Authentication error: Token may have expired");
				removeAuthTokens();
				toast.error("Сесія закінчилася. Будь ласка, увійдіть знову.");
			} else if (error.response.status === 403) {
				toast.error("У вас немає доступу до цього ресурсу.");
			} else if (error.response.status === 404) {
				toast.error("Ресурс не знайдено.");
			} else if (error.response.status >= 500) {
				toast.error("Помилка сервера. Будь ласка, спробуйте пізніше.");
			} else {
				toast.error(errorMessage);
			}
		} else if (error.request) {
			console.error("Network Error:", error.request);
			toast.error("Помилка мережі. Перевірте ваше з'єднання.");
		} else {
			console.error("Error:", error.message);
			toast.error("Невідома помилка");
		}

		return Promise.reject(error);
	}
);
```

### Hook Error Handling

```ts
// src/MentorSync.UI/src/features/auth/hooks/useUser.ts
export const useUser = () => {
	const { isAuthenticated } = useAuth();
	const [user, setUser] = useState<UserProfile | null>(null);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const fetchUserProfile = async () => {
			if (!isAuthenticated) {
				setUser(null);
				setLoading(false);
				return;
			}

			try {
				setLoading(true);
				const response = await api.get("/users/profile");
				setUser(response.data);
				setError(null);
			} catch (err) {
				console.error("Failed to fetch user profile:", err);
				setError("Failed to load user profile");
				setUser(null);
			} finally {
				setLoading(false);
			}
		};

		fetchUserProfile();
	}, [isAuthenticated]);

	return { user, loading, error };
};
```

### Component Error Handling

```tsx
// src/MentorSync.UI/src/features/mentor-profile/components/MentorReviewForm.tsx
const MentorReviewForm: React.FC<MentorReviewFormProps> = ({ mentorId }) => {
	const [isSubmitting, setIsSubmitting] = useState(false);
	const [submitError, setSubmitError] = useState<string | null>(null);

	const onSubmit: SubmitHandler<ReviewFormData> = async (data) => {
		setSubmitError(null);
		setIsSubmitting(true);

		try {
			const result = await createMentorReview(mentorId, data);

			if (result.success) {
				toast.success("Рецензія успішно опублікована");
				onReviewSubmitted?.();
			} else {
				setSubmitError(result.message || "Failed to submit review");
				toast.error(result.message || "Failed to submit review");
			}
		} catch (error) {
			const errorMessage =
				error instanceof Error
					? error.message
					: "An unexpected error occurred";
			setSubmitError(errorMessage);
			toast.error(errorMessage);
		} finally {
			setIsSubmitting(false);
		}
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			{submitError && (
				<div className="bg-red-50 border border-red-200 rounded p-4 mb-4">
					<p className="text-red-700">{submitError}</p>
				</div>
			)}
			{/* Form fields */}
		</form>
	);
};
```

## Toast Notification System

### Toast Configuration

```tsx
// src/MentorSync.UI/src/App.tsx
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
	return (
		<AuthProvider>
			<RouterProvider router={router} />
			<ToastContainer
				position="bottom-right"
				autoClose={3000}
				hideProgressBar={false}
				newestOnTop={true}
				closeOnClick
				rtl={false}
				pauseOnFocusLoss
				draggable
				pauseOnHover
				theme="light"
			/>
		</AuthProvider>
	);
}
```

### Toast Usage

```ts
import { toast } from "react-toastify";

// Error
toast.error("Помилка при завантаженні користувачів");

// Success
toast.success("Профіль успішно оновлено");

// Info
toast.info("Операція в прогресі...");

// Warning
toast.warning("Це видалить дані");
```

## Result<T> Pattern (Backend)

### Result Types

```cs
// Ardalis.Result library
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public IEnumerable<string> Errors { get; }
    public ResultStatus Status { get; } // Success, Invalid, NotFound, Error, etc.
}
```

### Result Usage in Handlers

```cs
public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
{
    // Validation
    if (string.IsNullOrWhiteSpace(command.Email))
    {
        return Result.Error("Email is required");
    }

    // Business logic
    var user = await userManager.FindByEmailAsync(command.Email);
    if (user is null)
    {
        return Result.NotFound("User not found");
    }

    // Success
    return Result.Success(new AuthResponse(...));
}
```

### Endpoint Error Response

```cs
private static async Task<IResult> Handle(
    LoginRequest request,
    IMediator mediator,
    CancellationToken cancellationToken)
{
    var command = new LoginCommand(request.Email, request.Password);
    var result = await mediator.SendCommandAsync<LoginCommand, AuthResponse>(
        command, cancellationToken);

    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.BadRequest(new { errors = result.Errors });
}
```

## Key Patterns

1. **Global Exception Handler**: Middleware catches all unhandled exceptions
2. **RFC 7807 Problem Details**: Standard error response format with trace ID
3. **Trace IDs**: Every error includes trace ID for debugging and support
4. **Toast Notifications**: All errors shown to user via toast, not console
5. **User-Friendly Messages**: Technical errors transformed to user-friendly text
6. **Error Logging**: Server-side logging with full exception details
7. **Status Code Handling**: Different messages for different HTTP status codes

## Error Categories

-   **400 Bad Request**: Validation errors, malformed requests
-   **401 Unauthorized**: Authentication required or token expired
-   **403 Forbidden**: User lacks permission for resource
-   **404 Not Found**: Resource doesn't exist
-   **500 Server Error**: Unhandled exceptions on backend
-   **Network Errors**: Client-side connectivity issues

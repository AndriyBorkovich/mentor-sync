using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MentorSync.API;

/// <summary>
/// Global exception handler for the MentorSync API that provides consistent error responses
/// </summary>
/// <param name="logger">Logger for exception handling events</param>
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
	/// <summary>
	/// Attempts to handle the exception and generate an appropriate HTTP response
	/// </summary>
	/// <param name="httpContext">The HTTP context for the current request</param>
	/// <param name="exception">The exception that occurred</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the exception was handled</returns>
	/// <example>
	/// <code>
	/// This handler automatically processes:
	/// - ValidationException -> 400 Bad Request with validation errors
	/// - OperationCanceledException -> 409 Conflict
	/// - Other exceptions -> 500 Internal Server Error
	/// </code>
	/// </example>
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var problemDetails = new ProblemDetails();
		switch (exception)
		{
			case ValidationException fluentException:
				{
					problemDetails.Title = "one or more validation errors occurred.";
					problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
					httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

					var validationErrors = fluentException.Errors.Select(error => error.ErrorMessage).ToList();
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
				break;
		}

		logger.LogError(exception, "{ProblemDetailsTitle}", problemDetails.Title);

		problemDetails.Status = httpContext.Response.StatusCode;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);

		return true;
	}
}

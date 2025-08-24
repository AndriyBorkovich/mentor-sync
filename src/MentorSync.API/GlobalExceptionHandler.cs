using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MentorSync.API;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
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

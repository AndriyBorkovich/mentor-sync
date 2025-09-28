using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace MentorSync.SharedKernel.Extensions;

/// <summary>
/// Extension methods for converting Ardalis.Result types to ASP.NET Core HTTP responses
/// </summary>
public static class ResultExtensions
{
	/// <summary>
	/// Converts a Result&lt;T&gt; to an appropriate HTTP response
	/// </summary>
	/// <typeparam name="T">The type of the result value</typeparam>
	/// <param name="result">The result to convert</param>
	/// <returns>An HTTP result appropriate for the result status</returns>
	/// <example>
	/// <code>
	/// var result = Result&lt;User&gt;.Success(user);
	/// return result.DecideWhatToReturn(); // Returns 200 OK with user data
	/// </code>
	/// </example>
	public static IResult DecideWhatToReturn<T>(this Result<T> result)
	{
		return result.Status switch
		{
			ResultStatus.Ok => Results.Ok(result.Value),
			ResultStatus.NotFound => CreateProblem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Resource Not Found",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Invalid => CreateProblem(
				statusCode: StatusCodes.Status400BadRequest,
				title: "Validation Error",
				detail: "One or more validation errors occurred.",
				extensions: new Dictionary<string, object> (StringComparer.OrdinalIgnoreCase)
				{
					["errors"] = result.ValidationErrors,
				}),
			ResultStatus.Unauthorized => CreateProblem(
				statusCode: StatusCodes.Status401Unauthorized,
				title: "Unauthorized",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Forbidden => CreateProblem(
				statusCode: StatusCodes.Status403Forbidden,
				title: "Forbidden",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Error => CreateProblem(
				statusCode: StatusCodes.Status500InternalServerError,
				title: "Operation Failed",
				detail: string.Join("; ", result.Errors)),
			ResultStatus.NoContent => CreateProblem(
				statusCode: StatusCodes.Status204NoContent,
				title: "No Content"),
			ResultStatus.Conflict => CreateProblem(
				statusCode: StatusCodes.Status409Conflict,
				title: "Conflict",
				detail: result.Errors.FirstOrDefault()),
			_ => CreateProblem(
				statusCode: StatusCodes.Status500InternalServerError,
				title: "Internal Server Error",
				detail: "An unexpected error occurred.")
		};
	}

	/// <summary>
	/// Converts a Result to an appropriate HTTP response
	/// </summary>
	/// <param name="result">The result to convert</param>
	/// <returns>An HTTP result appropriate for the result status</returns>
	public static IResult DecideWhatToReturn(this Result result)
	{
		return result.Status switch
		{
			ResultStatus.Ok => Results.Ok(result.Value),
			ResultStatus.NotFound => CreateProblem(
				statusCode: StatusCodes.Status404NotFound,
				title: "Resource Not Found",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Invalid => CreateProblem(
				statusCode: StatusCodes.Status400BadRequest,
				title: "Validation Error",
				detail: "One or more validation errors occurred.",
				extensions: new Dictionary<string, object> (StringComparer.OrdinalIgnoreCase)
				{
					["validationErrors"] = result.ValidationErrors
				}),
			ResultStatus.Unauthorized => CreateProblem(
				statusCode: StatusCodes.Status401Unauthorized,
				title: "Unauthorized",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Forbidden => CreateProblem(
				statusCode: StatusCodes.Status403Forbidden,
				title: "Forbidden",
				detail: result.Errors.FirstOrDefault()),
			ResultStatus.Error => CreateProblem(
				statusCode: StatusCodes.Status400BadRequest,
				title: "Operation Failed",
				detail: string.Join("; ", result.Errors)),
			_ => CreateProblem(
				statusCode: StatusCodes.Status500InternalServerError,
				title: "Internal Server Error",
				detail: "An unexpected error occurred.")
		};
	}

	private static IResult CreateProblem(
		int statusCode,
		string title,
		string detail = null,
		string type = null,
		string instance = null,
		IDictionary<string, object> extensions = null)
	{
		return Results.Problem(
			detail: detail,
			instance: instance,
			statusCode: statusCode,
			title: title,
			type: type ?? $"https://httpstatuses.com/{statusCode}",
			extensions: extensions);
	}
}

using Ardalis.Result;
using FluentValidation;

namespace MentorSync.SharedKernel.Behaviors;

/// <summary>
/// Pipeline behavior that validates input using FluentValidation validators
/// </summary>
/// <typeparam name="TInput">The type of input to validate</typeparam>
/// <typeparam name="TOutput">The type of output from the pipeline</typeparam>
/// <param name="validators">Collection of validators for the input type</param>
public sealed class ValidationBehavior<TInput, TOutput>(IEnumerable<IValidator<TInput>> validators)
	: IPipelineBehavior<TInput, TOutput>
{
	/// <summary>
	/// Handles the pipeline by validating the input before proceeding to the next handler
	/// </summary>
	/// <param name="input">The input to validate</param>
	/// <param name="next">The next handler in the pipeline</param>
	/// <param name="cancellationToken">Cancellation token for the operation</param>
	/// <returns>A task containing the result of the pipeline execution</returns>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public async Task<Result<TOutput>> Handle(TInput input, Func<Task<Result<TOutput>>> next, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(next);

		if (!validators.Any())
		{
			return await next().ConfigureAwait(false);
		}

		var context = new ValidationContext<TInput>(input);

		var validationResults = await Task.WhenAll(
			validators.Select(v =>
				v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

		var failures = validationResults
			.Where(r => r.Errors.Count > 0)
			.SelectMany(r => r.Errors)
			.ToList();

		if (failures.Count > 0)
		{
			throw new ValidationException(failures);
		}

		return await next().ConfigureAwait(false);
	}
}

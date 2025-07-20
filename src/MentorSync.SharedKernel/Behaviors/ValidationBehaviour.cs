using Ardalis.Result;
using FluentValidation;
using MentorSync.SharedKernel.Abstractions.Messaging;

namespace MentorSync.SharedKernel.Behaviors;

public sealed class ValidationBehavior<TInput, TOutput>(IEnumerable<IValidator<TInput>> validators)
    : IPipelineBehavior<TInput, TOutput>
{
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

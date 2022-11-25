using Common.SDK;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Polly;
using static Common.Application.Validation.Constants;

namespace Common.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : Result
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validator = _validators.FirstOrDefault();

        if (validator != null)
        {
            var retryPolicy = Policy<ValidationResult>
            .HandleResult(r => r.Errors.Any(x => (string)x.CustomState == DEPENDENCY))
            .WaitAndRetryAsync(4, retryAttempt =>
                TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100)
            );

            var validationResult = await retryPolicy.ExecuteAsync(async () =>
                await validator.ValidateAsync(request, cancellationToken)
            );

            if (validationResult.Errors.Any())
            {
                var errors = validationResult.Errors.Select(x => new Error(x.PropertyName, x.ErrorCode));
                var result = Activator.CreateInstance(typeof(TResponse), 422, errors);
                return (TResponse)result;
            }
        }
        return await next();
    }
}


using Common.Application.Idempotency;
using Common.SDK;
using MediatR;

namespace Common.Application.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : Result
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IIdempotencyCheck<TRequest>> _idempotencyCheckers;

    public IdempotencyBehavior(IEnumerable<IIdempotencyCheck<TRequest>> idempotencyCheckers)
    {
        _idempotencyCheckers = idempotencyCheckers;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var idempotencyChecker = _idempotencyCheckers.FirstOrDefault();

        if (idempotencyChecker != null)
        {
            var result = await idempotencyChecker.IsOperationAlreadyAppliedAsync(request, cancellationToken);
            if (result)
                return (TResponse)Activator.CreateInstance(typeof(TResponse), 204);
        }
        return await next();
    }
}


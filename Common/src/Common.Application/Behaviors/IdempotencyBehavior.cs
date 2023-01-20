using Common.Application.Idempotency;
using Common.SDK;
using MediatR;

namespace Common.Application.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : Result
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IIdempotencyCheck<TRequest>> _idempotencyChecks;

    public IdempotencyBehavior(IEnumerable<IIdempotencyCheck<TRequest>> idempotencyChecks)
    {
        _idempotencyChecks = idempotencyChecks;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var idempotencyCheck = _idempotencyChecks.FirstOrDefault();

        if (idempotencyCheck != null)
        {
            var result = await idempotencyCheck.IsOperationAlreadyAppliedAsync(request, cancellationToken);
            if (result)
                return (TResponse)Activator.CreateInstance(typeof(TResponse), 204);
        }
        return await next();
    }
}


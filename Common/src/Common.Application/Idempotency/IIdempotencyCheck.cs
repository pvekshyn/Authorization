namespace Common.Application.Idempotency;
public interface IIdempotencyCheck<TRequest>
{
    public Task<bool> IsOperationAlreadyAppliedAsync(TRequest request, CancellationToken cancellationToken);
}

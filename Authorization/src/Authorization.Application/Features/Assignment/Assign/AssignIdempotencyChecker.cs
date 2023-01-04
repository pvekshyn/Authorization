using Authorization.Application.Dependencies;
using Common.Application.Idempotency;

namespace Authorization.Application.Features.Assignment.Assign;

public class AssignIdempotencyChecker : IIdempotencyCheck<Assign>
{
    private readonly IAssignmentRepository _repository;

    public AssignIdempotencyChecker(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(Assign request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return _repository.IsAssignmentExist(request.Assignment.Id);
    }
}

using Assignment.Application.Dependencies;
using Common.Application.Idempotency;

namespace Assignment.Application.Features.Assign;

public class AssignIdempotencyCheck : IIdempotencyCheck<Assign>
{
    private readonly IAssignmentRepository _repository;

    public AssignIdempotencyCheck(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(Assign request, CancellationToken cancellationToken)
    {
        return await _repository.AnyAsync(request.Assignment.UserId, request.Assignment.RoleId, cancellationToken);
    }
}

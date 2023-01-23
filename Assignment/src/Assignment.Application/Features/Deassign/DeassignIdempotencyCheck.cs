using Assignment.Application.Dependencies;
using Common.Application.Idempotency;

namespace Assignment.Application.Features.Deassign;

public class DeassignIdempotencyCheck : IIdempotencyCheck<Deassign>
{
    private readonly IAssignmentRepository _repository;

    public DeassignIdempotencyCheck(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(Deassign request, CancellationToken cancellationToken)
    {
        var isAny = await _repository.AnyAsync(request.UserId, request.RoleId, cancellationToken);

        return !isAny;
    }
}

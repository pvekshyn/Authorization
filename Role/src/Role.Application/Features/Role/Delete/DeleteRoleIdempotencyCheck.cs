using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Delete;
public class DeleteRoleIdempotencyCheck : IIdempotencyCheck<DeleteRole>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleIdempotencyCheck(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(DeleteRole request, CancellationToken cancellationToken)
    {
        var roleExist = await _roleRepository.AnyAsync(request.Id, cancellationToken);

        return !roleExist;
    }
}

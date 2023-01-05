using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Rename;
public class RenameRoleIdempotencyCheck : IIdempotencyCheck<RenameRole>
{
    private readonly IRoleRepository _roleRepository;

    public RenameRoleIdempotencyCheck(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(RenameRole request, CancellationToken cancellationToken)
    {
        return await _roleRepository
            .AnyAsync(request.Role.Id, request.Role.Name, cancellationToken);
    }
}

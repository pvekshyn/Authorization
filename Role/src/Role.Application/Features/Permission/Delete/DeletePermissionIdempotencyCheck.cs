using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Delete;

public class DeletePermissionIdempotencyCheck : IIdempotencyCheck<DeletePermission>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionIdempotencyCheck(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(DeletePermission request, CancellationToken cancellationToken)
    {
        var permissionExist = await _permissionRepository
            .AnyAsync(request.PermissionId, cancellationToken);

        return !permissionExist;
    }
}

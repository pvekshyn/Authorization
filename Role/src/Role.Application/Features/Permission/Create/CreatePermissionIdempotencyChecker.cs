using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Create;

public class CreatePermissionIdempotencyCheck : IIdempotencyCheck<CreatePermission>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionIdempotencyCheck(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(CreatePermission request, CancellationToken cancellationToken)
    {
        return await _permissionRepository.AnyAsync(request.Permission.Id, cancellationToken);
    }
}

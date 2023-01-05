using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.UpdatePermissions;

public class UpdateRolePermissionsIdempotencyCheck : IIdempotencyCheck<UpdateRolePermissions>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRolePermissionsIdempotencyCheck(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(UpdateRolePermissions request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.Role.Id, cancellationToken);

        if (role == null)
            return false;

        var permissionIdsFromDb = role.Permissions.Select(x => x.Id.Value).ToList();

        return (request.Role.PermissionIds.Count == permissionIdsFromDb.Count) &&
            !request.Role.PermissionIds.Except(permissionIdsFromDb).Any();
    }
}

using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.UpdateRolePermissions;

public class UpdateRolePermissionsIdempotencyCheck : IIdempotencyCheck<UpdateRolePermissions>
{
    private readonly IRoleDbContext _dbContext;

    public UpdateRolePermissionsIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(UpdateRolePermissions request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .Include(x => x.Permissions)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Role.Id);

        if (role == null)
            return false;

        var permissionIdsFromDb = role.Permissions.Select(x => x.Id.Value).ToList();

        return (request.Role.PermissionIds.Count == permissionIdsFromDb.Count) &&
            !request.Role.PermissionIds.Except(permissionIdsFromDb).Any();
    }
}

using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.DeletePermission;

public class DeletePermissionIdempotencyCheck : IIdempotencyCheck<DeletePermission>
{
    private readonly IRoleDbContext _dbContext;

    public DeletePermissionIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(DeletePermission request, CancellationToken cancellationToken)
    {
        var permissionExist = await _dbContext.Permissions
            .AnyAsync(x => x.Id == request.PermissionId, cancellationToken);

        return !permissionExist;
    }
}

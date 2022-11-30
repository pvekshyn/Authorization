using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.DeleteRole;
public class DeleteRoleIdempotencyCheck : IIdempotencyCheck<DeleteRole>
{
    private readonly IRoleDbContext _dbContext;

    public DeleteRoleIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(DeleteRole request, CancellationToken cancellationToken)
    {
        var RoleExist = await _dbContext.Roles
            .AnyAsync(x => x.Id == request.Id, cancellationToken);

        return !RoleExist;
    }
}

using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.RenameRole;
public class RenameRoleIdempotencyCheck : IIdempotencyCheck<RenameRole>
{
    private readonly IRoleDbContext _dbContext;

    public RenameRoleIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(RenameRole request, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .AnyAsync(x => x.Id == request.Role.Id && x.Name == request.Role.Name, cancellationToken);
    }
}

using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Create;

public class CreatePermissionIdempotencyCheck : IIdempotencyCheck<CreatePermission>
{
    private readonly IRoleDbContext _dbContext;

    public CreatePermissionIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(CreatePermission request, CancellationToken cancellationToken)
    {
        return await _dbContext.Permissions
            .AnyAsync(x => x.Id == request.Permission.Id && x.Name == request.Permission.Name, cancellationToken);
    }
}

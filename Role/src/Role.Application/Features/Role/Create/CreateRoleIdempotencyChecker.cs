using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Create;

public class CreateRoleIdempotencyCheck : IIdempotencyCheck<CreateRole>
{
    private readonly IRoleDbContext _dbContext;

    public CreateRoleIdempotencyCheck(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(CreateRole request, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .AnyAsync(x => x.Id == request.Role.Id && x.Name == request.Role.Name, cancellationToken);
    }
}

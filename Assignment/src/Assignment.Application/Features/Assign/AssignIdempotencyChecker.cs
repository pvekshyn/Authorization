using Assignment.Application.Dependencies;
using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Features.Assign;

public class AssignIdempotencyChecker : IIdempotencyCheck<Assign>
{
    private readonly IAssignmentDbContext _dbContext;

    public AssignIdempotencyChecker(IAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(Assign request, CancellationToken cancellationToken)
    {
        return await _dbContext.Assignments
            .AnyAsync(x => x.UserId == request.Assignment.UserId &&
                x.RoleId == request.Assignment.RoleId, cancellationToken);
    }
}

using Assignment.Application.Dependencies;
using Common.Application.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Features.Deassign;

public class DeassignIdempotencyCheck : IIdempotencyCheck<Deassign>
{
    private readonly IAssignmentDbContext _dbContext;

    public DeassignIdempotencyCheck(IAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(Deassign request, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Assignments.AnyAsync(
            x => x.UserId == request.UserId &&
            x.RoleId == request.RoleId, cancellationToken);

        return !isAny;
    }
}

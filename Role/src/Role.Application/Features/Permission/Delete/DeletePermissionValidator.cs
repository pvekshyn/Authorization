using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Permission.DeletePermission;

public class DeletePermissionValidator : AbstractValidator<DeletePermission>
{
    private readonly IRoleDbContext _dbContext;

    public DeletePermissionValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.PermissionId)
            .MustAsync(NotLinkedToAnyRole).WithErrorCode(LINKED_TO_ROLE);
    }

    private async Task<bool> NotLinkedToAnyRole(Guid permissionId, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Roles
            .Include(x => x.Permissions)
            .AnyAsync(r => r.Permissions.Any(p => p.Id == permissionId), cancellationToken);

        return !isAny;
    }
}

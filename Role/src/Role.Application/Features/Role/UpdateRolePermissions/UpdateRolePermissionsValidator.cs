using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.UpdateRolePermissions;

public class UpdateRolePermissionsValidator : AbstractValidator<UpdateRolePermissions>
{
    private readonly IRoleDbContext _dbContext;

    public UpdateRolePermissionsValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Role.Id)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);

        RuleFor(x => x.Role.PermissionIds)
            .NotEmpty().WithErrorCode(EMPTY)
            .MustAsync(AreExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> AreExist(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var permissionsIds = await _dbContext.Permissions
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return permissionsIds.Count == ids.Count;
    }
}

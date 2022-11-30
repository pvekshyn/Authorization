using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.Domain;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRole>
{
    private readonly IRoleDbContext _dbContext;

    public CreateRoleValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Role.Id)
            .MustAsync(IsIdUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Role.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxRoleNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Role.PermissionIds)
            .NotEmpty()
            .MustAsync(AreExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> IsIdUnique(Guid id, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
        return !isAny;
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Roles.AnyAsync(x => x.Name == name, cancellationToken);
        return !isAny;
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

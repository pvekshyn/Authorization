using Role.Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Role.Application.Validation.Errors;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.CreatePermission;

public class CreatePermissionValidator : AbstractValidator<CreatePermission>
{
    private readonly IRoleDbContext _dbContext;

    public CreatePermissionValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Permission.Id)
            .MustAsync(IsIdUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Permission.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxPermissionNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);
    }

    private async Task<bool> IsIdUnique(Guid id, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Permissions.AnyAsync(x => x.Id == id, cancellationToken);
        return !isAny;
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Permissions.AnyAsync(x => x.Name == name, cancellationToken);
        return !isAny;
    }
}

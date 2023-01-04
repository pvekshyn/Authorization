using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.Domain;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.Rename;

public class RenameRoleValidator : AbstractValidator<RenameRole>
{
    private readonly IRoleDbContext _dbContext;

    public RenameRoleValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Role.Id)
           .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);

        RuleFor(x => x.Role.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxRoleNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _dbContext.Roles
            .AnyAsync(x => x.Name == name, cancellationToken);
        return !isAny;
    }
}

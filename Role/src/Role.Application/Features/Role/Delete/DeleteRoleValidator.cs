using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.Delete;

public class DeleteRoleValidator : AbstractValidator<DeleteRole>
{
    private readonly IRoleDbContext _dbContext;

    public DeleteRoleValidator(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }
}

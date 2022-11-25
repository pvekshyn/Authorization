using Assignment.Application.Dependencies;
using Common.Application.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Features.Assign;

public class AssignValidator : AbstractValidator<Assign>
{
    private readonly IAssignmentDbContext _dbContext;

    public AssignValidator(IAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Assignment.Id)
           .NotEmpty().WithErrorCode(EMPTY);

        RuleFor(x => x.Assignment.UserId)
           .NotEmpty().WithErrorCode(EMPTY);

        RuleFor(x => x.Assignment.RoleId)
            .NotEmpty().WithErrorCode(EMPTY)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND).WithDependency();
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }
}

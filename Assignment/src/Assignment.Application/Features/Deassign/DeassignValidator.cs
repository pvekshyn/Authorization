using Assignment.Application.Dependencies;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Features.Deassign;

public class DeassignValidator : AbstractValidator<Deassign>
{
    private readonly IAssignmentDbContext _dbContext;

    public DeassignValidator(IAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.RoleId)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND); ;
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }
}

using Assignment.Application.Dependencies;
using FluentValidation;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Features.Assign;

public class AssignValidator : AbstractValidator<Assign>
{
    private readonly IRoleRepository _roleRepository;

    public AssignValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.Assignment.Id)
           .NotEmpty().WithErrorCode(EMPTY);

        RuleFor(x => x.Assignment.UserId)
           .NotEmpty().WithErrorCode(EMPTY);

        RuleFor(x => x.Assignment.RoleId)
            .NotEmpty().WithErrorCode(EMPTY)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _roleRepository.AnyAsync(id, cancellationToken);
    }
}

using Assignment.Application.Dependencies;
using FluentValidation;
using static Assignment.Application.Validation.Errors;

namespace Assignment.Application.Features.Deassign;

public class DeassignValidator : AbstractValidator<Deassign>
{
    private readonly IRoleRepository _roleRepository;

    public DeassignValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.RoleId)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _roleRepository.AnyAsync(id, cancellationToken);
    }
}

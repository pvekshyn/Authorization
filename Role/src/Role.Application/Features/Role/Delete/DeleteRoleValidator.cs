using FluentValidation;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.Delete;

public class DeleteRoleValidator : AbstractValidator<DeleteRole>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.Id)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _roleRepository.AnyAsync(id, cancellationToken);
    }
}

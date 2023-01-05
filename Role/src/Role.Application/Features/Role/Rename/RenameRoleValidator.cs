using FluentValidation;
using Role.Application.Dependencies;
using Role.Domain;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.Rename;

public class RenameRoleValidator : AbstractValidator<RenameRole>
{
    private readonly IRoleRepository _roleRepository;

    public RenameRoleValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.Role.Id)
           .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);

        RuleFor(x => x.Role.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxRoleNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _roleRepository.AnyAsync(id, cancellationToken);
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _roleRepository.AnyAsync(name, cancellationToken);
        return !isAny;
    }
}

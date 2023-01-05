using Role.Domain;
using FluentValidation;
using static Role.Application.Validation.Errors;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Create;

public class CreatePermissionValidator : AbstractValidator<CreatePermission>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionValidator(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;

        RuleFor(x => x.Permission.Id)
            .MustAsync(IsIdUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Permission.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxPermissionNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);
    }

    private async Task<bool> IsIdUnique(Guid id, CancellationToken cancellationToken)
    {
        var isAny = await _permissionRepository.AnyAsync(id, cancellationToken);
        return !isAny;
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _permissionRepository.AnyAsync(name, cancellationToken);
        return !isAny;
    }
}

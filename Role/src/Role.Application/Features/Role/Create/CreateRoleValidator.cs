using FluentValidation;
using Role.Application.Dependencies;
using Role.Domain;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.Create;

public class CreateRoleValidator : AbstractValidator<CreateRole>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateRoleValidator(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;

        RuleFor(x => x.Role.Id)
            .MustAsync(IsIdUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Role.Name)
            .NotEmpty().WithErrorCode(EMPTY)
            .Length(0, Constants.MaxRoleNameLength).WithErrorCode(TOO_LONG)
            .MustAsync(IsNameUnique).WithErrorCode(ALREADY_EXIST);

        RuleFor(x => x.Role.PermissionIds)
            .NotEmpty()
            .MustAsync(AllExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> IsIdUnique(Guid id, CancellationToken cancellationToken)
    {
        var isAny = await _roleRepository.AnyAsync(id, cancellationToken);
        return !isAny;
    }

    private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        var isAny = await _roleRepository.AnyAsync(name, cancellationToken);
        return !isAny;
    }

    private async Task<bool> AllExist(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAsync(ids, cancellationToken);

        return permissions.Count == ids.Count;
    }
}

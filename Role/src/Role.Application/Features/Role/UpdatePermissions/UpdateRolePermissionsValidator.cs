using FluentValidation;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Role.UpdatePermissions;

public class UpdateRolePermissionsValidator : AbstractValidator<UpdateRolePermissions>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;

    public UpdateRolePermissionsValidator(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;

        RuleFor(x => x.Role.Id)
            .MustAsync(RoleExist).WithErrorCode(NOT_FOUND);

        RuleFor(x => x.Role.PermissionIds)
            .NotEmpty().WithErrorCode(EMPTY)
            .MustAsync(AllExist).WithErrorCode(NOT_FOUND);
    }

    private async Task<bool> RoleExist(Guid id, CancellationToken cancellationToken)
    {
        return await _roleRepository.AnyAsync(id, cancellationToken);
    }

    private async Task<bool> AllExist(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAsync(ids, cancellationToken);

        return permissions.Count == ids.Count;
    }
}

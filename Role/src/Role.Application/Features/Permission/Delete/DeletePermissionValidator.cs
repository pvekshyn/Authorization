using FluentValidation;
using Role.Application.Dependencies;
using static Role.Application.Validation.Errors;

namespace Role.Application.Features.Permission.Delete;

public class DeletePermissionValidator : AbstractValidator<DeletePermission>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionValidator(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;

        RuleFor(x => x.PermissionId)
            .MustAsync(NotLinkedToAnyRole).WithErrorCode(LINKED_TO_ROLE);
    }

    private async Task<bool> NotLinkedToAnyRole(Guid permissionId, CancellationToken cancellationToken)
    {
        var isAny = await _permissionRepository.IsLinkedToAnyRole(permissionId, cancellationToken);

        return !isAny;
    }
}

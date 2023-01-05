using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.UpdatePermissions;

public class UpdateRolePermissions : IRequest<Result>
{
    public UpdateRolePermissionsDto Role { get; init; }
    public UpdateRolePermissions(UpdateRolePermissionsDto role)
    {
        Role = role;
    }
}

public class UpdateRolePermissionsHandler : IRequestHandler<UpdateRolePermissions, Result>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;

    public UpdateRolePermissionsHandler(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(
        UpdateRolePermissions request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.Role.Id, cancellationToken);

        var permissions = await _permissionRepository.GetAsync(request.Role.PermissionIds, cancellationToken);

        role.ReplacePermissions(permissions);

        await _roleRepository.UpdatePermissionsAsync(role, cancellationToken);

        return Result.Ok();
    }

}

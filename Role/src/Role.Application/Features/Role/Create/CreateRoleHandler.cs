using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Role.Domain.ValueObjects.Role;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Create;

public class CreateRole : IRequest<Result>
{
    public CreateRoleDto Role { get; init; }
    public CreateRole(CreateRoleDto role)
    {
        Role = role;
    }
}

public class CreateRoleHandler : IRequestHandler<CreateRole, Result>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateRoleHandler(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(
        CreateRole request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAsync(request.Role.PermissionIds, cancellationToken);

        var role = new Domain.Role(
            new RoleId(request.Role.Id),
            new RoleName(request.Role.Name),
            permissions);

        await _roleRepository.CreateAsync(role, cancellationToken);

        return Result.Ok();
    }
}

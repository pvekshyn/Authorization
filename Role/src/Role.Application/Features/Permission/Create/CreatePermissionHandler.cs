using Role.Domain.ValueObjects.Permission;
using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Create;

public class CreatePermission : IRequest<Result>
{
    public PermissionDto Permission { get; init; }
    public CreatePermission(PermissionDto permission)
    {
        Permission = permission;
    }
}

public class CreatePermissionHandler : IRequestHandler<CreatePermission, Result>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionHandler(
        IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result> Handle(CreatePermission request, CancellationToken cancellationToken)
    {
        var permission = new Domain.Permission(
            new PermissionId(request.Permission.Id),
            new PermissionName(request.Permission.Name));

        await _permissionRepository.CreateAsync(permission, cancellationToken);

        return Result.Ok();
    }


}

using Common.SDK;
using MediatR;
using Role.Application.Dependencies;

namespace Role.Application.Features.Permission.Delete;

public class DeletePermission : IRequest<Result>
{
    public Guid PermissionId { get; init; }
    public DeletePermission(Guid permissionId)
    {
        PermissionId = permissionId;
    }
}

public class DeletePermissionHandler : IRequestHandler<DeletePermission, Result>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result> Handle(
        DeletePermission request, CancellationToken cancellationToken)
    {
        await _permissionRepository.DeleteAsync(request.PermissionId, cancellationToken);

        return Result.Ok();
    }
}

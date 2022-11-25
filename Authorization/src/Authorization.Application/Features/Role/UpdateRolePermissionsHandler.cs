using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Role
{
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
        private readonly IRolePermissionRepository _repository;

        public UpdateRolePermissionsHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateRolePermissions request, CancellationToken cancellationToken)
        {
            _repository.UpdateRolePermissions(request.Role);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

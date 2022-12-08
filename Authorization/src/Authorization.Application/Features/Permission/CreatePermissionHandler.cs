using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Permission
{
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
        private readonly IRolePermissionRepository _repository;

        public CreatePermissionHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(CreatePermission request, CancellationToken cancellationToken)
        {
            _repository.AddPermission(request.Permission);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

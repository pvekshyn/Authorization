using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Role
{
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
        private readonly IRolePermissionRepository _repository;

        public CreateRoleHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(CreateRole request, CancellationToken cancellationToken)
        {
            _repository.AddRole(request.Role);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

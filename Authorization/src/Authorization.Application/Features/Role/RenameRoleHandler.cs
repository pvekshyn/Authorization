using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;
using Role.SDK.DTO;

namespace Authorization.Application.Features.Role
{
    public class RenameRole : IRequest<Result>
    {
        public RenameRoleDto Role { get; init; }
        public RenameRole(RenameRoleDto role)
        {
            Role = role;
        }
    }
    public class RenameRoleHandler : IRequestHandler<RenameRole, Result>
    {
        private readonly IRolePermissionRepository _repository;

        public RenameRoleHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(RenameRole request, CancellationToken cancellationToken)
        {
            _repository.RenameRole(request.Role);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

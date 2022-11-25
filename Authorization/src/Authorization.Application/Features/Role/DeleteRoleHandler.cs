using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Authorization.Application.Features.Role
{
    public class DeleteRole : IRequest<Result>
    {
        public Guid Id { get; init; }
        public DeleteRole(Guid id)
        {
            Id = id;
        }
    }
    public class DeleteRoleHandler : IRequestHandler<DeleteRole, Result>
    {
        private readonly IRolePermissionRepository _repository;

        public DeleteRoleHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteRole request, CancellationToken cancellationToken)
        {
            _repository.DeleteRole(request.Id);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

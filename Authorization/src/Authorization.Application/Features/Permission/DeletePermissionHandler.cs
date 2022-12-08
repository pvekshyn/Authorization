using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Authorization.Application.Features.Permission
{
    public class DeletePermission : IRequest<Result>
    {
        public Guid Id { get; init; }
        public DeletePermission(Guid id)
        {
            Id = id;
        }
    }
    public class DeletePermissionHandler : IRequestHandler<DeletePermission, Result>
    {
        private readonly IRolePermissionRepository _repository;

        public DeletePermissionHandler(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeletePermission request, CancellationToken cancellationToken)
        {
            _repository.DeletePermission(request.Id);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

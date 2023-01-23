using Assignment.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Assignment.Application.Features.Role
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
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result> Handle(DeleteRole request, CancellationToken cancellationToken)
        {
            await _roleRepository.DeleteAsync(request.Id);

            return Result.Ok();
        }
    }
}

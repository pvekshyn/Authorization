using Assignment.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Assignment.Application.Features.Role
{
    public class CreateRole : IRequest<Result>
    {
        public Guid Id { get; init; }
        public CreateRole(Guid id)
        {
            Id = id;
        }
    }

    public class CreateRoleHandler : IRequestHandler<CreateRole, Result>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result> Handle(CreateRole request, CancellationToken cancellationToken)
        {
            await _roleRepository.CreateAsync(request.Id);
            return Result.Ok();
        }
    }
}

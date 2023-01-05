using Common.SDK;
using MediatR;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Delete;

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
        var role = await _roleRepository.GetAsync(request.Id, cancellationToken);

        if (role != null)
        {
            await _roleRepository.DeleteAsync(role, cancellationToken);
        }

        return Result.Ok();
    }


}

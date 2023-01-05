using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Role.Application.Dependencies;
using Role.Domain.ValueObjects.Role;

namespace Role.Application.Features.Role.Rename;

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
    private readonly IRoleRepository _roleRepository;

    public RenameRoleHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(
        RenameRole request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.Role.Id, cancellationToken);

        var newName = new RoleName(request.Role.Name);
        role.Rename(newName);

        await _roleRepository.RenameAsync(role, cancellationToken);

        return Result.Ok();
    }
}

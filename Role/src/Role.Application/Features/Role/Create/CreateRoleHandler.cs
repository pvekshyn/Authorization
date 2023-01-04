using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.Domain.ValueObjects.Role;
using Role.SDK.Events;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Create;

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
    private readonly IRoleDbContext _dbContext;

    public CreateRoleHandler(IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        CreateRole request, CancellationToken cancellationToken)
    {
        var permissions = await _dbContext.Permissions
            .Where(x => request.Role.PermissionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var role = new Domain.Role(
            new RoleId(request.Role.Id),
            new RoleName(request.Role.Name),
            permissions);

        var pubsubEvent = MapToEvent(role);

        await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
        await _dbContext.Roles.AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static RoleCreatedEvent MapToEvent(Domain.Role role)
    {
        return new RoleCreatedEvent
        {
            Role = new CreateRoleDto
            {
                Id = role.Id,
                Name = role.Name,
                PermissionIds = role.Permissions.Select(x => x.Id.Value).ToList()
            }
        };
    }
}

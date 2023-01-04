using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.SDK.Events;
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
    private readonly IRoleDbContext _dbContext;

    public RenameRoleHandler(
        IRoleDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    public async Task<Result> Handle(
        RenameRole request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .SingleAsync(x => x.Id == request.Role.Id, cancellationToken);

        var newName = new RoleName(request.Role.Name);
        role.Rename(newName);

        var pubsubEvent = MapToEvent(role);

        await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static RoleRenamedEvent MapToEvent(Domain.Role role)
    {
        return new RoleRenamedEvent
        {
            Role = new RenameRoleDto
            {
                Id = role.Id,
                Name = role.Name
            }
        };
    }
}

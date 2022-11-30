using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.SDK.Events;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.UpdateRolePermissions;

public class UpdateRolePermissions : IRequest<Result>
{
    public UpdateRolePermissionsDto Role { get; init; }
    public UpdateRolePermissions(UpdateRolePermissionsDto role)
    {
        Role = role;
    }
}

public class UpdateRolePermissionsHandler : IRequestHandler<UpdateRolePermissions, Result>
{
    private readonly IRoleDbContext _dbContext;

    public UpdateRolePermissionsHandler(
        IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateRolePermissions request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .Include(x => x.Permissions)
            .SingleAsync(x => x.Id == request.Role.Id, cancellationToken);

        var permissions = await _dbContext.Permissions
            .Where(x => request.Role.PermissionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var pubsubEvent = MapToEvent(role);

        role.ReplacePermissions(permissions);
        await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static RolePermissionsChangedEvent MapToEvent(Domain.Role role)
    {
        return new RolePermissionsChangedEvent
        {
            Role = new UpdateRolePermissionsDto
            {
                Id = role.Id,
                PermissionIds = role.Permissions.Select(x => x.Id.Value).ToList()
            }
        };
    }
}

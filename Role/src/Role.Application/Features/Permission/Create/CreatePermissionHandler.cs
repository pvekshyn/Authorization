using Role.Domain.ValueObjects.Permission;
using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Role.Application.Dependencies;
using Role.SDK.Events;

namespace Role.Application.Features.Permission.Create;

public class CreatePermission : IRequest<Result>
{
    public PermissionDto Permission { get; init; }
    public CreatePermission(PermissionDto permission)
    {
        Permission = permission;
    }
}

public class CreatePermissionHandler : IRequestHandler<CreatePermission, Result>
{
    private readonly IRoleDbContext _dbContext;

    public CreatePermissionHandler(
        IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(CreatePermission request, CancellationToken cancellationToken)
    {
        var permission = new Domain.Permission(
            new PermissionId(request.Permission.Id),
            new PermissionName(request.Permission.Name));

        var pubsubEvent = MapToEvent(permission);

        await _dbContext.AddPubSubOutboxMessageAsync(permission.Id, pubsubEvent, cancellationToken);
        await _dbContext.Permissions.AddAsync(permission, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static PermissionCreatedEvent MapToEvent(Domain.Permission permission)
    {
        return new PermissionCreatedEvent
        {
            Permission = new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name
            }
        };
    }
}

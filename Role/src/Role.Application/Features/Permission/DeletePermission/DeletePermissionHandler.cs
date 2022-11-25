using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.SDK.Events;

namespace Role.Application.Features.Permission.DeletePermission;

public class DeletePermission : IRequest<Result>
{
    public Guid PermissionId { get; init; }
    public DeletePermission(Guid permissionId)
    {
        PermissionId = permissionId;
    }
}

public class DeletePermissionHandler : IRequestHandler<DeletePermission, Result>
{
    private readonly IRoleDbContext _dbContext;
    private readonly IMediator _mediator;

    public DeletePermissionHandler(
        IRoleDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Result> Handle(
        DeletePermission request, CancellationToken cancellationToken)
    {
        var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == request.PermissionId, cancellationToken);

        if (permission != null)
        {
            var pubsubEvent = MapToEvent(permission);

            await _dbContext.AddPubSubOutboxMessageAsync(permission.Id, pubsubEvent, cancellationToken);

            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok();
    }

    private static PermissionDeletedEvent MapToEvent(Domain.Permission permission)
    {
        return new PermissionDeletedEvent
        {
            Id = permission.Id
        };
    }
}

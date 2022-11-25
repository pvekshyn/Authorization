using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.SDK.Events;

namespace Role.Application.Features.Role.DeleteRole;

public class DeleteRole : IRequest<Result>
{
    public Guid RoleId { get; init; }
    public DeleteRole(Guid roleId)
    {
        RoleId = roleId;
    }
}

public class DeleteRoleHandler : IRequestHandler<DeleteRole, Result>
{
    private readonly IRoleDbContext _dbContext;
    private readonly IMediator _mediator;

    public DeleteRoleHandler(
        IRoleDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Result> Handle(
        DeleteRole request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);

        if (role != null)
        {
            var pubsubEvent = MapToEvent(role);

            await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok();
    }

    private static RoleDeletedEvent MapToEvent(Domain.Role role)
    {
        return new RoleDeletedEvent
        {
            Id = role.Id
        };
    }
}

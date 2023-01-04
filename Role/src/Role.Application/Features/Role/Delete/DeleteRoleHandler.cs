using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.SDK.Events;

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
    private readonly IRoleDbContext _dbContext;

    public DeleteRoleHandler(
        IRoleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteRole request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

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

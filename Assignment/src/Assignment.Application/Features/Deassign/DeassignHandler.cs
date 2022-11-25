using Assignment.Application.Dependencies;
using Assignment.SDK.DTO;
using Assignment.SDK.Events;
using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Features.Deassign;

public class Deassign : IRequest<Result>
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public Deassign(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}

public class DeassignHandler : IRequestHandler<Deassign, Result>
{
    private readonly IAssignmentDbContext _dbContext;

    public DeassignHandler(
        IAssignmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(Deassign request, CancellationToken cancellationToken)
    {
        var assignment = await _dbContext.Assignments.SingleOrDefaultAsync(x =>
            x.UserId == request.UserId &&
            x.RoleId == request.RoleId, cancellationToken);

        if (assignment != null)
        {
            var deassignmentEvent = MapToEvent(assignment);

            _dbContext.Assignments.Remove(assignment);
            await _dbContext.AddPubSubOutboxMessageAsync(assignment.Id, deassignmentEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok();
    }

    private static DeassignmentEvent MapToEvent(Domain.Assignment assignment)
    {
        return new DeassignmentEvent
        {
            Assignment = new AssignmentDto
            {
                Id = assignment.Id,
                UserId = assignment.UserId,
                RoleId = assignment.RoleId
            }
        };
    }
}

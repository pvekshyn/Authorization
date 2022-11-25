using Common.SDK;
using MediatR;
using Assignment.SDK.DTO;
using Assignment.Domain.ValueObjects;
using Assignment.Application.Dependencies;
using Assignment.SDK.Events;

namespace Assignment.Application.Features.Assign;

public class Assign : IRequest<Result>
{
    public AssignmentDto Assignment { get; init; }
    public Assign(AssignmentDto assignment)
    {
        Assignment = assignment;
    }
}

public class AssignHandler : IRequestHandler<Assign, Result>
{
    private readonly IAssignmentDbContext _dbContext;
    private readonly IMediator _mediator;

    public AssignHandler(
        IAssignmentDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Result> Handle(Assign request, CancellationToken cancellationToken)
    {
        var assignmentId = new AssignmentId(request.Assignment.Id);
        var userId = new UserId(request.Assignment.UserId);
        var roleId = new RoleId(request.Assignment.RoleId);
        var assignment = new Domain.Assignment(assignmentId, userId, roleId);

        var assignmentEvent = MapToEvent(assignment);

        await _dbContext.Assignments.AddAsync(assignment, cancellationToken);
        await _dbContext.AddPubSubOutboxMessageAsync(assignment.Id, assignmentEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static AssignmentEvent MapToEvent(Domain.Assignment assignment)
    {
        return new AssignmentEvent
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

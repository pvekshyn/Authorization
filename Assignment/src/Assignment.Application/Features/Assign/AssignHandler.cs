using Common.SDK;
using MediatR;
using Assignment.SDK.DTO;
using Assignment.Domain.ValueObjects;
using Assignment.Application.Dependencies;

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
    private readonly IAssignmentRepository _repository;

    public AssignHandler(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(Assign request, CancellationToken cancellationToken)
    {
        var assignmentId = new AssignmentId(request.Assignment.Id);
        var userId = new UserId(request.Assignment.UserId);
        var roleId = new RoleId(request.Assignment.RoleId);
        var assignment = new Domain.Assignment(assignmentId, userId, roleId);

        await _repository.AssignAsync(assignment, cancellationToken);

        return Result.Ok();
    }
}

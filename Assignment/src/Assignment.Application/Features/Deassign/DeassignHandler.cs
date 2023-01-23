using Assignment.Application.Dependencies;
using Common.SDK;
using MediatR;

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
    private readonly IAssignmentRepository _repository;

    public DeassignHandler(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(Deassign request, CancellationToken cancellationToken)
    {
        await _repository.DeassignAsync(request.UserId, request.RoleId, cancellationToken);

        return Result.Ok();
    }
}

using Assignment.SDK.DTO;
using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Authorization.Application.Features.Assignment
{
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
            _repository.AddAssignment(request.Assignment);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

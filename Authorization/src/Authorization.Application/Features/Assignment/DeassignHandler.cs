using Authorization.Application.Dependencies;
using Common.SDK;
using MediatR;

namespace Authorization.Application.Features.Assignment
{
    public class Deassign : IRequest<Result>
    {
        public Guid Id { get; init; }
        public Deassign(Guid id)
        {
            Id = id;
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
            _repository.DeleteAssignment(request.Id);
            await Task.CompletedTask;

            return Result.Ok();
        }
    }
}

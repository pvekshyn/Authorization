using Assignment.Application.Dependencies;
using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Features.Role
{
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
        private readonly IAssignmentDbContext _dbContext;

        public DeleteRoleHandler(IAssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteRole request, CancellationToken cancellationToken)
        {
            //ToDo replace this check with idempotency check
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (role is not null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }

            return Result.Ok();
        }
    }
}

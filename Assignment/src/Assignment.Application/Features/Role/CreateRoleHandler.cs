using Assignment.Application.Dependencies;
using Assignment.Domain.ValueObjects;
using Common.SDK;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Features.Role
{
    public class CreateRole : IRequest<Result>
    {
        public Guid Id { get; init; }
        public CreateRole(Guid id)
        {
            Id = id;
        }
    }

    public class CreateRoleHandler : IRequestHandler<CreateRole, Result>
    {
        private readonly IAssignmentDbContext _dbContext;

        public CreateRoleHandler(IAssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(CreateRole request, CancellationToken cancellationToken)
        {
            //ToDo replace this check with idempotency check
            var alreadyExist = await _dbContext.Roles.AnyAsync(x => x.Id == request.Id);
            if (!alreadyExist)
            {
                var role = new Domain.Role(new RoleId(request.Id));
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
            }

            return Result.Ok();
        }
    }
}

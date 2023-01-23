using Assignment.Application.Dependencies;
using Assignment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly AssignmentDbContext _dbContext;

        public RoleRepository(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task CreateAsync(Guid id)
        {
            //ToDo replace this check with idempotency check
            var alreadyExist = await _dbContext.Roles.AnyAsync(x => x.Id == id);
            if (!alreadyExist)
            {
                var role = new Domain.Role(new RoleId(id));
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            //ToDo replace this check with idempotency check
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role is not null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

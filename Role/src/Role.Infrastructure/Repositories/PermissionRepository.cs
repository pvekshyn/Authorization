using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Role.Application.Dependencies;
using Role.Domain;
using Role.SDK.DTO;
using Role.SDK.Events;
using System.Data;

namespace Role.Infrastructure.Repositories
{
    internal class PermissionRepository : IPermissionRepository
    {
        private readonly RoleDbContext _dbContext;
        private readonly ICapPublisher _capBus;

        public PermissionRepository(RoleDbContext dbContext, ICapPublisher capBus)
        {
            _dbContext = dbContext;
            _capBus = capBus;
        }

        public async Task<ICollection<Permission>> GetAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _dbContext.Permissions
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }


        public async Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Permissions.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Permissions.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> IsLinkedToAnyRole(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .Include(x => x.Permissions)
                .AnyAsync(r => r.Permissions.Any(p => p.Id == id), cancellationToken);
        }

        public async Task CreateAsync(Permission permission, CancellationToken cancellationToken)
        {
            var pubsubEvent = MapToCreatedEvent(permission);

            using (var trans = _dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                await _dbContext.Permissions.AddAsync(permission, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _capBus.Publish(typeof(PermissionCreatedEvent).FullName, pubsubEvent);
            }
        }

        public async Task DeleteAsync(Guid permissionId, CancellationToken cancellationToken)
        {
            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionId, cancellationToken);

            if (permission != null)
            {
                var pubsubEvent = MapToDeletedEvent(permission);

                using (var trans = _dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    _dbContext.Permissions.Remove(permission);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    _capBus.Publish(typeof(PermissionDeletedEvent).FullName, pubsubEvent);
                }
            }
        }

        private static PermissionCreatedEvent MapToCreatedEvent(Permission permission)
        {
            return new PermissionCreatedEvent
            {
                Permission = new PermissionDto
                {
                    Id = permission.Id,
                    Name = permission.Name
                }
            };
        }

        private static PermissionDeletedEvent MapToDeletedEvent(Permission permission)
        {
            return new PermissionDeletedEvent
            {
                Id = permission.Id
            };
        }

    }
}

﻿using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;
using Role.SDK.Events;
using Role.Application.Dependencies;

namespace Role.Infrastructure.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly RoleDbContext _dbContext;

        public RoleRepository(RoleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Role?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .Include(x => x.Permissions)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> AnyAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> AnyAsync(Guid id, string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.AnyAsync(x => x.Id == id && x.Name == name, cancellationToken);
        }

        public async Task CreateAsync(Domain.Role role, CancellationToken cancellationToken)
        {
            var pubsubEvent = MapToCreatedEvent(role);

            await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
            await _dbContext.Roles.AddAsync(role, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RenameAsync(Domain.Role role, CancellationToken cancellationToken)
        {
            var pubsubEvent = MapToRenamedEvent(role);

            await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdatePermissionsAsync(Domain.Role role, CancellationToken cancellationToken)
        {
            var pubsubEvent = MapToPermissionsUpdatedEvent(role);

            await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Domain.Role role, CancellationToken cancellationToken)
        {
            var pubsubEvent = MapToDeletedEvent(role);

            await _dbContext.AddPubSubOutboxMessageAsync(role.Id, pubsubEvent, cancellationToken);
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static RoleCreatedEvent MapToCreatedEvent(Domain.Role role)
        {
            return new RoleCreatedEvent
            {
                Role = new CreateRoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    PermissionIds = role.Permissions.Select(x => x.Id.Value).ToList()
                }
            };
        }

        private static RoleRenamedEvent MapToRenamedEvent(Domain.Role role)
        {
            return new RoleRenamedEvent
            {
                Role = new RenameRoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                }
            };
        }

        private static RolePermissionsChangedEvent MapToPermissionsUpdatedEvent(Domain.Role role)
        {
            return new RolePermissionsChangedEvent
            {
                Role = new UpdateRolePermissionsDto
                {
                    Id = role.Id,
                    PermissionIds = role.Permissions.Select(x => x.Id.Value).ToList()
                }
            };
        }

        private static RoleDeletedEvent MapToDeletedEvent(Domain.Role role)
        {
            return new RoleDeletedEvent
            {
                Id = role.Id
            };
        }
    }
}

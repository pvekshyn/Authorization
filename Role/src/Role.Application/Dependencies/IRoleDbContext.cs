using Role.Domain;
using Microsoft.EntityFrameworkCore;

namespace Role.Application.Dependencies;
public interface IRoleDbContext
{
    DbSet<Permission> Permissions { get; set; }
    DbSet<Domain.Role> Roles { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task AddPubSubOutboxMessageAsync(Guid entityId, object pubSubEvent, CancellationToken cancellationToken);
}

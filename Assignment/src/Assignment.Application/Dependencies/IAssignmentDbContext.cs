using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Dependencies;
public interface IAssignmentDbContext
{
    DbSet<Domain.Assignment> Assignments { get; set; }
    DbSet<Domain.Role> Roles { get; set; }

    Task AddPubSubOutboxMessageAsync(Guid entityId, object pubSubEvent, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

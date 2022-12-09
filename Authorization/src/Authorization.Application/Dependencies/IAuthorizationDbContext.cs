using Authorization.Domain;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.DataAccess.Read;

public interface IAuthorizationDbContext
{
    DbSet<Permission> Permissions { get; set; }
    DbSet<Domain.Role> Roles { get; set; }
    DbSet<AssignmentViewEntry> AssignmentViewEntries { get; set; }
}

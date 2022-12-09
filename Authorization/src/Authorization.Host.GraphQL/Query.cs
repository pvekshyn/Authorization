using Authorization.Domain;
using Authorization.Infrastructure.DataAccess.Read;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Host.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<AssignmentViewEntry> GetAssignments([Service] IAuthorizationDbContext context) =>
            context.AssignmentViewEntries.AsNoTracking();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Domain.Role> GetRoles([Service] IAuthorizationDbContext context) =>
            context.Roles.AsNoTracking();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Permission> GetPermissions([Service] IAuthorizationDbContext context) =>
            context.Permissions.AsNoTracking();
    }
}

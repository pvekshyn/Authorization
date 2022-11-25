using Authorization.Infrastructure.DataAccess.Read;
using Authorization.Infrastructure.DataAccess.Read.Models;

namespace Authorization.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<AssignmentViewEntry> GetAssignments([Service] IAuthorizationDbContext context) =>
            context.AssignmentViewEntries;

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Infrastructure.DataAccess.Read.Models.Role> GetRoles([Service] IAuthorizationDbContext context) =>
            context.Roles;
    }
}

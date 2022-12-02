using Role.Domain.ValueObjects.Role;
using Role.Infrastructure;

namespace Role.Integration.Tests
{
    internal static class TestExtensions
    {
        public static Guid CreateRole(this RoleDbContext dbContext)
        {
            var roleId = Guid.NewGuid();

            var permissions = dbContext.Permissions
                .Where(x => x.Id == Constants.AssignPermissionId)
                .ToList();

            var role = new Domain.Role(
                new RoleId(roleId),
                new RoleName($"Test {roleId}".Substring(0, 25)),
                permissions
                );

            dbContext.Roles.Add(role);
            dbContext.SaveChanges();

            return roleId;
        }

        public static void ShouldContain(this string actualString,
                                         string expectedSubString)
        {
            Assert.Contains(expectedSubString, actualString);
        }
    }
}

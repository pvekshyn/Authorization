using AutoFixture;
using Role.Domain.ValueObjects.Permission;
using Role.Domain.ValueObjects.Role;

namespace Role.Domain.Tests
{
    public class RoleTests : DomainTestBase
    {
        [Fact]
        public void NewRole_NullPermissions_Exception()
        {
            var roleId = _fixture.Create<Guid>();
            var roleName = _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength);

            Assert.Throws<ArgumentException>(() =>
                new Role(new RoleId(roleId), new RoleName(roleName), null)
            );
        }

        [Fact]
        public void NewRole_EmptyPermissions_Exception()
        {
            var roleId = _fixture.Create<Guid>();
            var roleName = _fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength);

            Assert.Throws<ArgumentException>(() =>
                new Role(new RoleId(roleId), new RoleName(roleName), Array.Empty<Permission>())
            );
        }

        [Fact]
        public void ReplacePermissions_NullPermissions_Exception()
        {
            var permission = new Permission(
                new PermissionId(_fixture.Create<Guid>()),
                new PermissionName(_fixture.Create<string>()));

            var role = new Role(
                new RoleId(_fixture.Create<Guid>()),
                new RoleName(_fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)),
                new List<Permission> { permission });

            Assert.Throws<ArgumentException>(() =>
                role.ReplacePermissions(null)
            );
        }

        [Fact]
        public void ReplacePermissions_EmptyPermissions_Exception()
        {
            var permission = new Permission(
                new PermissionId(_fixture.Create<Guid>()),
                new PermissionName(_fixture.Create<string>()));

            var role = new Role(
                new RoleId(_fixture.Create<Guid>()),
                new RoleName(_fixture.Create<string>().Substring(0, Constants.MaxRoleNameLength)),
                new List<Permission> { permission });

            Assert.Throws<ArgumentException>(() =>
                role.ReplacePermissions(Array.Empty<Permission>())
            );
        }
    }
}
using Role.Domain.ValueObjects.Role;

namespace Role.Domain.Tests.ValueObjects.Role
{
    public class RoleNameTests : DomainTestBase
    {
        [Fact]
        public void NewRoleName_Null_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new RoleName(null)
            );
        }

        [Fact]
        public void NewRoleName_EmptyString_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new RoleName(string.Empty)
            );
        }

        [Fact]
        public void NewRoleName_TooLong_Exception()
        {
            var roleName = new string('*', Constants.MaxRoleNameLength + 1);
            Assert.Throws<ArgumentException>(() =>
                new RoleName(roleName)
            );
        }

    }
}
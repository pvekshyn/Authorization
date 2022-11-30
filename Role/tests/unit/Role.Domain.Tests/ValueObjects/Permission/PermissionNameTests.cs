using Role.Domain.ValueObjects.Permission;

namespace Role.Domain.Tests.ValueObjects.Permission
{
    public class PermissionNameTests : DomainTestBase
    {
        [Fact]
        public void NewPermissionName_Null_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new PermissionName(null)
            );
        }

        [Fact]
        public void NewPermissionName_EmptyString_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new PermissionName(string.Empty)
            );
        }

        [Fact]
        public void NewPermissionName_TooLong_Exception()
        {
            var roleName = new string('*', Constants.MaxPermissionNameLength + 1);
            Assert.Throws<ArgumentException>(() =>
                new PermissionName(roleName)
            );
        }

    }
}
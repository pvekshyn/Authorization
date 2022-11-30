using Role.Domain.ValueObjects.Role;

namespace Role.Domain.Tests.ValueObjects.Role
{
    public class RoleIdTests : DomainTestBase
    {
        [Fact]
        public void NewRoleId_EmptyGuid_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new RoleId(Guid.Empty)
            );
        }
    }
}
using Role.Domain.ValueObjects.Permission;

namespace Role.Domain.Tests.ValueObjects.Permission
{
    public class PermissionIdTests : DomainTestBase
    {
        [Fact]
        public void NewPermissionId_EmptyGuid_Exception()
        {
            Assert.Throws<ArgumentException>(() =>
                new PermissionId(Guid.Empty)
            );
        }
    }
}
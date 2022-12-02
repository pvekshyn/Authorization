using Microsoft.EntityFrameworkCore;

namespace Role.Integration.Tests.Role
{
    public class DeleteRoleTests : IntegrationTestBase
    {
        public DeleteRoleTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task DeleteRole_NotExistingRole_Idempotent()
        {
            // Arrange
            var roleId = Guid.NewGuid();

            // Act
            var result = await _roleApiClient.DeleteRoleAsync(roleId, CancellationToken.None);

            // Assert
            Assert.Equal(204, result.Status);
        }

        [Fact]
        public async Task DeleteRole_Success()
        {
            // Arrange
            var roleId = _dbContext.CreateRole();

            // Act
            var result = await _roleApiClient.DeleteRoleAsync(roleId, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var roleExists = await _dbContext.Roles
                .AnyAsync(x => x.Id == roleId);

            Assert.False(roleExists);

            var outboxMessageExists = OutboxMessageExists(roleId);
            Assert.True(outboxMessageExists);
        }
    }
}
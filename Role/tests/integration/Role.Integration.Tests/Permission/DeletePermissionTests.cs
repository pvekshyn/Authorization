using Microsoft.EntityFrameworkCore;
using Role.Domain.ValueObjects.Permission;

namespace Role.Integration.Tests.Permission
{
    public class DeletePermissionTests : IntegrationTestBase
    {
        private const string _permissionName = "Test Delete Permission";

        public DeletePermissionTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task DeletePermission_NotExist_Idempotent()
        {
            // Arrange
            var permissionId = Guid.NewGuid();
            // Act
            var result = await _permissionApiClient.DeleteAsync(permissionId, CancellationToken.None);

            // Assert
            Assert.Equal(204, result.Status);
        }

        [Fact]
        public async Task DeletePermission_Success()
        {
            // Arrange
            var permission = CreatePermission();
            _dbContext.Permissions.Add(permission);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _permissionApiClient.DeleteAsync(permission.Id, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var permissionExists = await _dbContext.Permissions.AnyAsync(x => x.Id == permission.Id);
            Assert.False(permissionExists);

            var outboxMessageExists = OutboxMessageExists(permission.Id);
            Assert.True(outboxMessageExists);
        }

        private Domain.Permission CreatePermission()
        {
            var permissionId = Guid.NewGuid();
            return new Domain.Permission(
                new PermissionId(permissionId),
                new PermissionName($"Test {permissionId}"));
        }
    }
}
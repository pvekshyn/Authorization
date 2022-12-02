using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Role.Integration.Tests.Role
{
    public class UpdateRolePermissionsTests : IntegrationTestBase
    {
        public UpdateRolePermissionsTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task UpdateRolePermissions_NotExistingRole_Validation()
        {
            // Arrange
            var roleDto = GetRoleDto();

            // Act
            var result = await _roleApiClient.UpdateRolePermissionsAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(422, result.Status);
        }

        [Fact]
        public async Task UpdateRolePermissions_Success()
        {
            // Arrange
            var roleId = _dbContext.CreateRole();

            var roleDto = GetRoleDto(roleId);

            // Act
            var result = await _roleApiClient.UpdateRolePermissionsAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var role = await _dbContext.Roles
                .Include(x => x.Permissions)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == roleDto.Id);

            Assert.NotNull(role);
            Assert.Equal(roleDto.PermissionIds.Single(), role.Permissions.Single().Id);

            var outboxMessageExists = OutboxMessageExists(roleDto.Id);
            Assert.True(outboxMessageExists);
        }

        private UpdateRolePermissionsDto GetRoleDto(Guid? roleId = null)
        {
            return new UpdateRolePermissionsDto()
            {
                Id = roleId ?? Guid.NewGuid(),
                PermissionIds = new List<Guid> { Constants.DeassignPermissionId },
            };
        }
    }
}
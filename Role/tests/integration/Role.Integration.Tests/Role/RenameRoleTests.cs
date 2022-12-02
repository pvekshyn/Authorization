using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Role.Integration.Tests.Role
{
    public class RenameRoleTests : IntegrationTestBase
    {
        public RenameRoleTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task RenameRole_NotExistingRole_Validation()
        {
            // Arrange
            var roleDto = GetRoleDto();

            // Act
            var result = await _roleApiClient.RenameRoleAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(422, result.Status);
        }

        [Fact]
        public async Task RenameRole_Success()
        {
            // Arrange
            var roleId = _dbContext.CreateRole();

            var roleDto = GetRoleDto(roleId);

            // Act
            var result = await _roleApiClient.RenameRoleAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var role = await _dbContext.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == roleDto.Id);

            Assert.NotNull(role);
            Assert.Equal(roleDto.Name, role.Name);

            var outboxMessageExists = OutboxMessageExists(roleDto.Id);
            Assert.True(outboxMessageExists);
        }

        private RenameRoleDto GetRoleDto(Guid? roleId = null)
        {
            return new RenameRoleDto()
            {
                Id = roleId ?? Guid.NewGuid(),
                Name = $"Test {Guid.NewGuid()}".Substring(0, 25),
            };
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Role.Integration.Tests.Role
{
    public class CreateRoleTests : IntegrationTestBase
    {
        public CreateRoleTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task CreateRole_EmptyName_Validation()
        {
            // Arrange
            var roleDto = GetRoleDto(string.Empty);

            // Act
            var result = await _roleApiClient.CreateAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(422, result.Status);
        }

        [Fact]
        public async Task CreateRole_Success()
        {
            // Arrange
            var roleDto = GetRoleDto();

            // Act
            var result = await _roleApiClient.CreateAsync(roleDto, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var roleExists = await _dbContext.Roles.AnyAsync(x => x.Id == roleDto.Id);
            Assert.True(roleExists);

            var outboxMessageExists = OutboxMessageExists(roleDto.Id);
            Assert.True(outboxMessageExists);
        }

        [Fact]
        public async Task CreateRoleConcurrent_SameRole_OneSuccessOtherIdempotent()
        {
            // Arrange
            var roleDto = GetRoleDto();

            var task1 = _roleApiClient.CreateAsync(roleDto, CancellationToken.None);
            var task2 = _roleApiClient.CreateAsync(roleDto, CancellationToken.None);

            // Act
            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 204));

            var roleExists = await _dbContext.Roles.AnyAsync(x => x.Name == roleDto.Name);
            Assert.True(roleExists);

            var outboxMessageExists = OutboxMessageExists(roleDto.Id);
            Assert.True(outboxMessageExists);
        }

        [Fact]
        public async Task CreateRoleConcurrent_SameName_OneSuccessOthersValidation()
        {
            // Arrange
            var roleDto1 = GetRoleDto();
            var roleDto2 = GetRoleDto(roleDto1.Name);

            var task1 = _roleApiClient.CreateAsync(roleDto1, CancellationToken.None);
            var task2 = _roleApiClient.CreateAsync(roleDto2, CancellationToken.None);

            // Act
            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 422));

            var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == roleDto1.Name);
            Assert.NotNull(role);

            var outboxMessageExists = OutboxMessageExists(role.Id);
            Assert.True(outboxMessageExists);
        }


        private CreateRoleDto GetRoleDto(string? name = null)
        {
            var roleId = Guid.NewGuid();
            return new CreateRoleDto()
            {
                Id = roleId,
                Name = name ?? $"Test {roleId}".Substring(0, 25),
                PermissionIds = new[] { Constants.AssignPermissionId }
            };
        }
    }
}
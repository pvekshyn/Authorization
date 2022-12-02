using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Role.Integration.Tests.Permission
{
    public class CreatePermissionTests : IntegrationTestBase
    {
        public CreatePermissionTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task CreatePermission_EmptyName_Validation()
        {
            // Arrange
            var permissionDto = GetPermissionDto(string.Empty);

            // Act
            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            // Assert
            Assert.Equal(422, result.Status);
        }

        [Fact]
        public async Task CreatePermission_Success()
        {
            // Arrange
            var permissionDto = GetPermissionDto();

            // Act
            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionDto.Id);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permissionDto.Id);
            Assert.True(outboxMessageExists);
        }

        [Fact]
        public async Task CreatePermissionConcurrent_SamePermission_OneSuccessOtherIdempotent()
        {
            // Arrange
            var permissionDto = GetPermissionDto();

            var task1 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            // Act
            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 204));

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Name == permissionDto.Name);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permissionDto.Id);
            Assert.True(outboxMessageExists);
        }

        [Fact]
        public async Task CreatePermissionConcurrent_SameName_OneSuccessOthersValidation()
        {
            // Arrange
            var permissionDto1 = GetPermissionDto();
            var permissionDto2 = GetPermissionDto(permissionDto1.Name);

            var task1 = _permissionApiClient.CreateAsync(permissionDto1, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto2, CancellationToken.None);

            // Act
            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 422));

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Name == permissionDto1.Name);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permission.Id);
            Assert.True(outboxMessageExists);
        }

        private PermissionDto GetPermissionDto(string? name = null)
        {
            var permissionId = Guid.NewGuid();
            return new PermissionDto()
            {
                Id = permissionId,
                Name = name ?? $"Test {permissionId}",
            };
        }
    }
}
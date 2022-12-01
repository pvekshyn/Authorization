using Microsoft.EntityFrameworkCore;
using Role.SDK.DTO;

namespace Role.Integration.Tests.Permission
{
    public class CreatePermissionTests : IntegrationTestBase
    {
        private const string _permissionName = "Test Create Permission";

        public CreatePermissionTests(CustomWebApplicationFactory<Program> apiFactory) : base(apiFactory)
        {
        }

        [Fact]
        public async Task CreatePermission_EmptyName_Validation()
        {
            // Arrange
            var permissionDto = new PermissionDto()
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
            };

            // Act
            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            // Assert
            Assert.Equal(422, result.Status);
        }

        [Fact]
        public async Task CreatePermission_Success()
        {
            // Arrange
            var permissionDto = new PermissionDto()
            {
                Id = Guid.NewGuid(),
                Name = _permissionName,
            };

            // Act
            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.Status);

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionDto.Id);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permissionDto.Id);
            Assert.True(outboxMessageExists);

            //Cleanup
            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync();

            DeleteOutboxMessages(permissionDto.Id);
        }

        [Fact]
        public async Task CreatePermissionConcurrent_Samepermission_OneSuccessOtherIdempotent()
        {
            // Arrange
            var permissionDto = new PermissionDto()
            {
                Id = Guid.NewGuid(),
                Name = _permissionName,
            };

            // Act
            var task1 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 204));

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Name == _permissionName);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permissionDto.Id);
            Assert.True(outboxMessageExists);

            //Cleanup
            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync();

            DeleteOutboxMessages(permissionDto.Id);
        }

        [Fact]
        public async Task CreatePermissionConcurrent_SameName_OneSuccessOthersValidation()
        {
            // Arrange
            var permissionDto1 = new PermissionDto()
            {
                Id = Guid.NewGuid(),
                Name = _permissionName,
            };
            var permissionDto2 = new PermissionDto()
            {
                Id = Guid.NewGuid(),
                Name = _permissionName,
            };

            var task1 = _permissionApiClient.CreateAsync(permissionDto1, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto2, CancellationToken.None);

            // Act
            var results = await Task.WhenAll(task1, task2);

            // Assert
            Assert.Single(results.Where(x => x.Status == 200));
            Assert.Single(results.Where(x => x.Status == 422));

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Name == _permissionName);
            Assert.NotNull(permission);

            var outboxMessageExists = OutboxMessageExists(permission.Id);
            Assert.True(outboxMessageExists);

            //Cleanup
            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync();

            DeleteOutboxMessages(permission.Id);
        }
    }
}
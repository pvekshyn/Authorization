using Common.SDK;
using Microsoft.Extensions.Options;
using Polly;
using Refit;
using Role.SDK.DTO;
using Role.SDK.Features;
using SpecFlowTests.Support;

namespace SpecFlowTests.Drivers
{
    public class AuthorizationDriver
    {
        private IReadPermissionApi _permissionApiClient;
        private IReadRoleApi _roleApiClient;
        private ICheckAccessApi _checkAccessApiClient;

        public AuthorizationDriver(IOptions<TestSettings> testSettings)
        {
            var authorizationUrl = $"{testSettings.Value.IngressUrl}/authorization";
            _permissionApiClient = RestService.For<IReadPermissionApi>(authorizationUrl);
            _roleApiClient = RestService.For<IReadRoleApi>(authorizationUrl);
            _checkAccessApiClient = RestService.For<ICheckAccessApi>(authorizationUrl);
        }

        public async Task<Result<PermissionDto>> GetCreatedPermissionAsync(Guid id)
        {
            var retryPolicy = Policy<Result<PermissionDto>>
                .HandleResult(r => r.IsSuccess == false)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _permissionApiClient.GetPermissionAsync(id)
            );
        }

        public async Task<Result<PermissionDto>> GetDeletedPermissionAsync(Guid id)
        {
            var retryPolicy = Policy<Result<PermissionDto>>
                .HandleResult(r => r.IsSuccess)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _permissionApiClient.GetPermissionAsync(id)
            );
        }

        public async Task<Result<RoleDto>> GetCreatedRoleAsync(Guid id)
        {
            var retryPolicy = Policy<Result<RoleDto>>
                .HandleResult(r => r.IsSuccess == false)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _roleApiClient.GetRoleAsync(id)
            );
        }

        public async Task<Result<RoleDto>> GetDeletedRoleAsync(Guid id)
        {
            var retryPolicy = Policy<Result<RoleDto>>
                .HandleResult(r => r.IsSuccess)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _roleApiClient.GetRoleAsync(id)
            );
        }

        public async Task<bool> CheckGotAccessAsync(Guid userId, Guid permissionId)
        {
            var retryPolicy = Policy<bool>
                .HandleResult(x => x == false)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _checkAccessApiClient.CheckAccessAsync(userId, permissionId)
            );
        }

        public async Task<bool> CheckLostAccessAsync(Guid userId, Guid permissionId)
        {
            var retryPolicy = Policy<bool>
                .HandleResult(x => x == true)
                .WaitAndRetry5TimesAsync();

            return await retryPolicy.ExecuteAsync(async () =>
                await _checkAccessApiClient.CheckAccessAsync(userId, permissionId)
            );
        }
    }
}

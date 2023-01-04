using Refit;
using Role.SDK.DTO;
using Role.SDK.Features;

namespace SpecFlowTests.Drivers
{
    public class RoleDriver
    {
        private IRoleApi _roleApiClient;
        private IPermissionApi _permissionApiClient;

        private FeatureContext _featureContext;


        public RoleDriver(FeatureContext featureContext)
        {
            var roleUrl = "http://localhost:8080/role";

            _roleApiClient = RestService.For<IRoleApi>(roleUrl);
            _permissionApiClient = RestService.For<IPermissionApi>(roleUrl);
            _featureContext = featureContext;
        }

        public async Task CreatePermissionAsync()
        {
            var permissionId = Guid.NewGuid();
            var permissionName = $"Test {permissionId}";

            _featureContext["permissionId"] = permissionId;
            _featureContext["permissionName"] = permissionName;

            var permissionDto = new PermissionDto
            {
                Id = permissionId,
                Name = permissionName
            };

            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);
            Assert.Equal(200, result.Status);
        }

        public async Task DeletePermissionAsync(Guid id)
        {
            var result = await _permissionApiClient.DeleteAsync(id, CancellationToken.None);
            Assert.Equal(200, result.Status);
        }

        public async Task CreateRoleAsync(Guid permissionId)
        {
            var roleId = Guid.NewGuid();
            var roleName = $"Test {roleId}".Substring(0, 25);

            _featureContext["roleId"] = roleId;
            _featureContext["roleName"] = roleName;

            var createRoleDto = new CreateRoleDto
            {
                Id = roleId,
                Name = roleName,
                PermissionIds = new List<Guid> { permissionId }
            };

            var result = await _roleApiClient.CreateAsync(createRoleDto, CancellationToken.None);
            Assert.Equal(200, result.Status);
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var result = await _roleApiClient.DeleteRoleAsync(id, CancellationToken.None);
            Assert.Equal(200, result.Status);
        }
    }
}

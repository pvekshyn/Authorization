using Refit;
using Role.SDK.DTO;
using Role.SDK.Features;

namespace SpecFlowTests.Drivers
{
    public class RoleDriver
    {
        private IRoleApi _roleApiClient;
        private IPermissionApi _permissionApiClient;

        private ScenarioContext _scenarioContext;


        public RoleDriver(ScenarioContext scenarioContext)
        {
            var roleUrl = "http://localhost:8080/role";

            _scenarioContext = scenarioContext;

            var token = (string)_scenarioContext["accessToken"];

            var settings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token),
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            _roleApiClient = RestService.For<IRoleApi>(roleUrl, settings);
            _permissionApiClient = RestService.For<IPermissionApi>(roleUrl, settings);

        }

        public async Task CreatePermissionAsync()
        {
            var permissionId = Guid.NewGuid();
            var permissionName = $"Test {permissionId}";

            _scenarioContext["permissionId"] = permissionId;
            _scenarioContext["permissionName"] = permissionName;

            var permissionDto = new PermissionDto
            {
                Id = permissionId,
                Name = permissionName
            };

            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);
            _scenarioContext["result"] = result;
        }

        public async Task DeletePermissionAsync(Guid id)
        {
            var result = await _permissionApiClient.DeleteAsync(id, CancellationToken.None);
            _scenarioContext["result"] = result;
        }

        public async Task CreateRoleAsync(Guid permissionId)
        {
            var roleId = Guid.NewGuid();
            var roleName = $"Test {roleId}".Substring(0, 25);

            _scenarioContext["roleId"] = roleId;
            _scenarioContext["roleName"] = roleName;

            var createRoleDto = new CreateRoleDto
            {
                Id = roleId,
                Name = roleName,
                PermissionIds = new List<Guid> { permissionId }
            };

            var result = await _roleApiClient.CreateAsync(createRoleDto, CancellationToken.None);
            _scenarioContext["result"] = result;
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var result = await _roleApiClient.DeleteRoleAsync(id, CancellationToken.None);
            _scenarioContext["result"] = result;
        }
    }
}

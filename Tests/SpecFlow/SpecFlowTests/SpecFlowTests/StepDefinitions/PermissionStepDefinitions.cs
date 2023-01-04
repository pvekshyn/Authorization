using SpecFlowTests.Drivers;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class PermissionStepDefinitions
    {
        private readonly RoleDriver _roleDriver;
        private readonly AuthorizationDriver _authorizationDriver;
        private FeatureContext _featureContext;

        public PermissionStepDefinitions(RoleDriver roleDriver, AuthorizationDriver authorizationDriver, FeatureContext featureContext)
        {
            _roleDriver = roleDriver;
            _authorizationDriver = authorizationDriver;
            _featureContext = featureContext;
        }

        [Given(@"permission created")]
        [When(@"permission created")]
        public async Task PermissionCreated()
        {
            await _roleDriver.CreatePermissionAsync();
        }

        [Then(@"permission in authorization service")]
        public async Task ThenPermissionInAuthorizationService()
        {
            var permissionId = (Guid)_featureContext["permissionId"];
            var permissionName = (string)_featureContext["permissionName"];

            var result = await _authorizationDriver.GetCreatedPermissionAsync(permissionId);
            Assert.Equal(200, result.Status);

            var permission = result.Data;
            Assert.Equal(permissionName, permission.Name);
        }

        [When(@"permission deleted")]
        public async Task WhenPermissionDeleted()
        {
            var permissionId = (Guid)_featureContext["permissionId"];
            await _roleDriver.DeletePermissionAsync(permissionId);
        }

        [Then(@"permission not in authorization service")]
        public async Task ThenPermissionNotInAuthorizationService()
        {
            var permissionId = (Guid)_featureContext["permissionId"];
            var result = await _authorizationDriver.GetDeletedPermissionAsync(permissionId);
            Assert.Equal(404, result.Status);
        }
    }
}

using SpecFlowTests.Drivers;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class RoleStepDefinitions
    {
        private readonly RoleDriver _roleDriver;
        private readonly AuthorizationDriver _authorizationDriver;
        private FeatureContext _featureContext;

        public RoleStepDefinitions(RoleDriver roleDriver, AuthorizationDriver authorizationDriver, FeatureContext featureContext)
        {
            _roleDriver = roleDriver;
            _authorizationDriver = authorizationDriver;
            _featureContext = featureContext;
        }

        [Given(@"role with this permission created")]
        [When(@"role with this permission created")]
        public async Task GivenRoleWithThisPermissionCreated()
        {
            var permissionId = (Guid)_featureContext["permissionId"];
            await _roleDriver.CreateRoleAsync(permissionId);
        }

        [Given(@"role in authorization service")]
        [Then(@"role in authorization service")]
        public async Task RoleInAuthorizationService()
        {
            var roleId = (Guid)_featureContext["roleId"];
            var roleName = (string)_featureContext["roleName"];

            var result = await _authorizationDriver.GetCreatedRoleAsync(roleId);
            Assert.Equal(200, result.Status);

            var permission = result.Data;
            Assert.Equal(roleName, permission.Name);

        }

        [When(@"role deleted")]
        public async Task WhenRoleDeleted()
        {
            var roleId = (Guid)_featureContext["roleId"];
            await _roleDriver.DeleteRoleAsync(roleId);

        }

        [Then(@"role not in authorization service")]
        public async Task ThenRoleNotInAuthorizationService()
        {
            var roleId = (Guid)_featureContext["roleId"];
            var result = await _authorizationDriver.GetDeletedRoleAsync(roleId);
            Assert.Equal(404, result.Status);
        }
    }
}

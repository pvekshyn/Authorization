using NUnit.Framework;
using SpecFlowTests.Drivers;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class PermissionStepDefinitions
    {
        private readonly RoleDriver _roleDriver;
        private readonly AuthorizationDriver _authorizationDriver;
        private ScenarioContext _scenarioContext;

        public PermissionStepDefinitions(RoleDriver roleDriver, AuthorizationDriver authorizationDriver, ScenarioContext scenarioContext)
        {
            _roleDriver = roleDriver;
            _authorizationDriver = authorizationDriver;
            _scenarioContext = scenarioContext;

            _scenarioContext["permissionId"] = Guid.Empty;
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
            var permissionId = (Guid)_scenarioContext["permissionId"];
            var permissionName = (string)_scenarioContext["permissionName"];

            var result = await _authorizationDriver.GetCreatedPermissionAsync(permissionId);
            Assert.AreEqual(200, result.Status);

            var permission = result.Data;
            Assert.AreEqual(permissionName, permission.Name);
        }

        [When(@"permission deleted")]
        public async Task WhenPermissionDeleted()
        {
            var permissionId = (Guid)_scenarioContext["permissionId"];
            await _roleDriver.DeletePermissionAsync(permissionId);
        }

        [Then(@"permission not in authorization service")]
        public async Task ThenPermissionNotInAuthorizationService()
        {
            var permissionId = (Guid)_scenarioContext["permissionId"];
            var result = await _authorizationDriver.GetDeletedPermissionAsync(permissionId);
            Assert.AreEqual(404, result.Status);
        }
    }
}

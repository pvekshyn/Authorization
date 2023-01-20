using SpecFlowTests.Drivers;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class RoleStepDefinitions
    {
        private readonly RoleDriver _roleDriver;
        private readonly AuthorizationDriver _authorizationDriver;
        private ScenarioContext _scenarioContext;

        public RoleStepDefinitions(
            RoleDriver roleDriver,
            AuthorizationDriver authorizationDriver,
            ScenarioContext scenarioContext)
        {
            _roleDriver = roleDriver;
            _authorizationDriver = authorizationDriver;
            _scenarioContext = scenarioContext;

            _scenarioContext["roleId"] = Guid.Empty;
        }

        [When(@"role created")]
        public async Task RoleCreated()
        {
            await _roleDriver.CreateRoleAsync(Guid.NewGuid());
        }

        [Given(@"role with this permission created")]
        [When(@"role with this permission created")]
        public async Task GivenRoleWithThisPermissionCreated()
        {
            var permissionId = (Guid)_scenarioContext["permissionId"];
            await _roleDriver.CreateRoleAsync(permissionId);
        }

        [Given(@"role in authorization service")]
        [Then(@"role in authorization service")]
        public async Task RoleInAuthorizationService()
        {
            var roleId = (Guid)_scenarioContext["roleId"];
            var roleName = (string)_scenarioContext["roleName"];

            var result = await _authorizationDriver.GetCreatedRoleAsync(roleId);
            Assert.Equal(200, result.Status);

            var permission = result.Data;
            Assert.Equal(roleName, permission.Name);

        }

        [When(@"role deleted")]
        public async Task WhenRoleDeleted()
        {
            var roleId = (Guid)_scenarioContext["roleId"];
            await _roleDriver.DeleteRoleAsync(roleId);

        }

        [Then(@"role not in authorization service")]
        public async Task ThenRoleNotInAuthorizationService()
        {
            var roleId = (Guid)_scenarioContext["roleId"];
            var result = await _authorizationDriver.GetDeletedRoleAsync(roleId);
            Assert.Equal(404, result.Status);
        }
    }
}

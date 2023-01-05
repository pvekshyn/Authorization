using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class DeleteRoleStepDefinitions : IntegrationTestBase
    {
        private ScenarioContext _scenarioContext;

        public DeleteRoleStepDefinitions(CustomWebApplicationFactory<Program> apiFactory, ScenarioContext scenarioContext) : base(apiFactory)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"Role not exist")]
        public void GivenRoleNotExist()
        {
            var roleId = Guid.NewGuid();
            _scenarioContext["roleId"] = roleId;
        }

        [When(@"Role deleted")]
        public async Task WhenRoleDeleted()
        {
            var roleId = (Guid)_scenarioContext["roleId"];

            var result = await _roleApiClient.DeleteRoleAsync(roleId, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = roleId;
        }
    }
}

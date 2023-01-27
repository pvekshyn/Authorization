using Role.SDK.Features;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class DeleteRoleStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private IRoleApi _roleApiClient;


        public DeleteRoleStepDefinitions(ScenarioContext scenarioContext, IRoleApi roleApiClient)
        {
            _scenarioContext = scenarioContext;
            _roleApiClient = roleApiClient;
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

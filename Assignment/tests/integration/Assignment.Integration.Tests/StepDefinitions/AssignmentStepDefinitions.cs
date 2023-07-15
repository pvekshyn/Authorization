using Assignment.SDK.DTO;
using Assignment.SDK.Features;
using Role.SDK.DTO;
using Role.SDK.Events;
using TechTalk.SpecFlow;

namespace Assignment.Integration.Tests.StepDefinitions
{
    [Binding]
    public class AssignmentStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        protected readonly IAssignmentApi _apiClient;
        protected readonly IEventProcessingApi _eventProcessingClient;

        public AssignmentStepDefinitions(
            ScenarioContext scenarioContext,
            IAssignmentApi apiClient,
            IEventProcessingApi eventProcessingClient)
        {
            _scenarioContext = scenarioContext;
            _scenarioContext["userId"] = Guid.NewGuid();

            _apiClient = apiClient;
            _eventProcessingClient = eventProcessingClient;
        }

        [Given(@"role not exist")]
        public void GivenRoleNotExist()
        {
            _scenarioContext["roleId"] = Guid.NewGuid();
        }

        [Given(@"user not assigned to role")]
        public void GivenUserNotAssignedToRole()
        {
            _scenarioContext["roleId"] = Guid.NewGuid();
        }

        [Given(@"role created")]
        public async Task GivenRoleCreated()
        {
            var roleId = Guid.NewGuid();
            var roleCreatedEvent = new RoleCreatedEvent
            {
                Role = new CreateRoleDto { Id = roleId, Name = "r", PermissionIds = new List<Guid> { Guid.NewGuid() } }
            };

            await _eventProcessingClient.RoleCreated(roleCreatedEvent);

            _scenarioContext["roleId"] = roleId;
        }

        [Given(@"user assigned to this role")]
        [When(@"user assigned to this role")]
        public async Task UserAssignedToThisRole()
        {
            var request = new AssignmentDto()
            {
                Id = Guid.NewGuid(),
                RoleId = (Guid)_scenarioContext["roleId"],
                UserId = (Guid)_scenarioContext["userId"]
            };

            var result = await _apiClient.AssignAsync(request);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = request.Id;
        }

        [When(@"user deassigned from this role")]
        public async Task WhenUserDeassignedFromThisRole()
        {
            var userId = (Guid)_scenarioContext["userId"];
            var roleId = (Guid)_scenarioContext["roleId"];

            var result = await _apiClient.DeassignAsync(userId, roleId);
            _scenarioContext["result"] = result;
        }
    }
}

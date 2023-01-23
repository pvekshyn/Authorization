extern alias API;
extern alias GRPC;

using Assignment.SDK.DTO;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Role.SDK.DTO;
using Role.SDK.Events;
using TechTalk.SpecFlow;

namespace Assignment.Integration.Tests.StepDefinitions
{
    [Binding]
    public class AssignmentStepDefinitions : IntegrationTestBase
    {
        private ScenarioContext _scenarioContext;

        public AssignmentStepDefinitions(
            CustomWebApplicationFactory<API.Program> apiFactory,
            CustomWebApplicationFactory<GRPC.Program> grpcFactory,
            ScenarioContext scenarioContext) : base(apiFactory, grpcFactory)
        {
            _scenarioContext = scenarioContext;

            _scenarioContext["userId"] = Guid.NewGuid();
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
        public void GivenRoleCreated()
        {
            var roleId = Guid.NewGuid();
            var roleCreatedEvent = new RoleCreatedEvent
            {
                Role = new CreateRoleDto { Id = roleId }
            };

            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            var roleCreatedEventString = JsonConvert.SerializeObject(roleCreatedEvent, settings);

            _grpcClient.ProcessEvent(new GrpcEventRequest() { Event = roleCreatedEventString });

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

        [Then(@"outbox message in database")]
        public void ThenOutboxMessageInDatabase()
        {
            var entityId = (Guid)_scenarioContext["entityId"];

            var sql = @"SELECT TOP 1 1 
                FROM OutboxMessage
                WHERE EntityId = @entityId";

            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                var outboxMessageExists = connection.QueryFirstOrDefault<bool>(sql, new { entityId });

                Assert.True(outboxMessageExists);
            }
        }
    }
}

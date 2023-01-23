using Assignment.SDK.DTO;
using Assignment.SDK.Features;
using Common.SDK;
using Polly;
using Refit;
using SpecFlowTests.Support;

namespace SpecFlowTests.Drivers
{
    public class AssignmentDriver
    {
        private IAssignmentApi _assignmentApiClient;

        private ScenarioContext _scenarioContext;


        public AssignmentDriver(ScenarioContext scenarioContext)
        {
            var assignmentUrl = "http://localhost:8080/assignment";

            var settings = new RefitSettings
            {
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            _assignmentApiClient = RestService.For<IAssignmentApi>(assignmentUrl, settings);
            _scenarioContext = scenarioContext;
        }

        public async Task AssignAsync(Guid roleId)
        {
            var userId = Guid.NewGuid();

            _scenarioContext["userId"] = userId;

            var assignmentDto = new AssignmentDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId
            };

            var retryPolicy = Policy<Result>
                .HandleResult(r => r.IsSuccess == false)
                .WaitAndRetry5TimesAsync();

            var result = await retryPolicy.ExecuteAsync(async () =>
                await _assignmentApiClient.AssignAsync(assignmentDto)
            );

            Assert.Equal(200, result.Status);
        }

        public async Task DeassignAsync(Guid userId, Guid roleId)
        {
            var retryPolicy = Policy<Result>
                .HandleResult(r => r.IsSuccess == false)
                .WaitAndRetry5TimesAsync();

            var result = await retryPolicy.ExecuteAsync(async () =>
                await _assignmentApiClient.DeassignAsync(userId, roleId)
            );

            Assert.Equal(200, result.Status);
        }
    }
}

using Assignment.SDK.DTO;
using Assignment.SDK.Features;
using Common.SDK;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Polly;
using Refit;
using SpecFlowTests.Support;

namespace SpecFlowTests.Drivers
{
    public class AssignmentDriver
    {
        private IAssignmentApi _assignmentApiClient;
        private ScenarioContext _scenarioContext;
        private string _ingressUrl;

        public AssignmentDriver(ScenarioContext scenarioContext, IOptions<TestSettings> testSettings)
        {
            _scenarioContext = scenarioContext;
            _ingressUrl = testSettings.Value.IngressUrl;

            var assignmentUrl = $"{_ingressUrl}/assignment-api";

            var token = (string)_scenarioContext["accessToken"];

            var settings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token),
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            _assignmentApiClient = RestService.For<IAssignmentApi>(assignmentUrl, settings);
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

            Assert.AreEqual(200, result.Status);
        }

        public async Task DeassignAsync(Guid userId, Guid roleId)
        {
            var retryPolicy = Policy<Result>
                .HandleResult(r => r.IsSuccess == false)
                .WaitAndRetry5TimesAsync();

            var result = await retryPolicy.ExecuteAsync(async () =>
                await _assignmentApiClient.DeassignAsync(userId, roleId)
            );

            Assert.AreEqual(200, result.Status);
        }
    }
}

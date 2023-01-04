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

        private FeatureContext _featureContext;


        public AssignmentDriver(FeatureContext featureContext)
        {
            var assignmentUrl = "http://localhost:8080/assignment";

            _assignmentApiClient = RestService.For<IAssignmentApi>(assignmentUrl);
            _featureContext = featureContext;
        }

        public async Task AssignAsync(Guid roleId)
        {
            var userId = Guid.NewGuid();

            _featureContext["userId"] = userId;

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

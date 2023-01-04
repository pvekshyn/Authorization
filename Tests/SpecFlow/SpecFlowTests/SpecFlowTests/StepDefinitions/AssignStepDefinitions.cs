using SpecFlowTests.Drivers;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class AssignStepDefinitions
    {
        private readonly AssignmentDriver _assignmentDriver;
        private readonly AuthorizationDriver _authorizationDriver;
        private FeatureContext _featureContext;

        public AssignStepDefinitions(AssignmentDriver assignmentDriver,
            AuthorizationDriver authorizationDriver,
            FeatureContext featureContext)
        {
            _assignmentDriver = assignmentDriver;
            _authorizationDriver = authorizationDriver;
            _featureContext = featureContext;
        }

        [Given(@"user assigned to this role")]
        [When(@"user assigned to this role")]
        public async Task UserAssignedToThisRole()
        {
            var roleId = (Guid)_featureContext["roleId"];
            await _assignmentDriver.AssignAsync(roleId);
        }

        [Given(@"user got access")]
        [Then(@"user got access")]
        public async Task ThenUserGotAccess()
        {
            var userId = (Guid)_featureContext["userId"];
            var permissionId = (Guid)_featureContext["permissionId"];
            var result = await _authorizationDriver.CheckGotAccessAsync(userId, permissionId);
            Assert.True(result);
        }

        [When(@"user deassigned from this role")]
        public async Task WhenUserDeassignedFromThisRole()
        {
            var userId = (Guid)_featureContext["userId"];
            var roleId = (Guid)_featureContext["roleId"];
            await _assignmentDriver.DeassignAsync(userId, roleId);
        }

        [Then(@"user lost access")]
        public async Task ThenUserLostAccess()
        {
            var userId = (Guid)_featureContext["userId"];
            var permissionId = (Guid)_featureContext["permissionId"];
            var result = await _authorizationDriver.CheckLostAccessAsync(userId, permissionId);
            Assert.False(result);
        }
    }
}

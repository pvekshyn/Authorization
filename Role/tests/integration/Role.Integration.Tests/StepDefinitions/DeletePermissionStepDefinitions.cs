using Role.Domain.ValueObjects.Permission;
using Role.Infrastructure;
using Role.SDK.Features;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class DeletePermissionStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly RoleDbContext _dbContext;
        private IPermissionApi _permissionApiClient;

        public DeletePermissionStepDefinitions(
            ScenarioContext scenarioContext,
            RoleDbContext dbContext,
            IPermissionApi permissionApiClient)
        {
            _scenarioContext = scenarioContext;
            _dbContext = dbContext;
            _permissionApiClient = permissionApiClient;
        }

        [Given(@"Permission not exist")]
        public void GivenPermissionNotExist()
        {
            var permissionId = Guid.NewGuid();
            _scenarioContext["permissionId"] = permissionId;
        }

        [Given(@"Permission exist")]
        public async Task GivenPermissionExist()
        {
            var permission = CreatePermission();
            _dbContext.Permissions.Add(permission);
            await _dbContext.SaveChangesAsync();

            _scenarioContext["permissionId"] = permission.Id.Value;
        }

        [Given(@"New permission exist")]
        public async Task GivenNewPermissionExist()
        {
            var permission = CreatePermission();
            _dbContext.Permissions.Add(permission);
            await _dbContext.SaveChangesAsync();

            _scenarioContext["newPermissionId"] = permission.Id.Value;
        }


        [When(@"Permission deleted")]
        public async Task WhenPermissionDeleted()
        {
            var permissionId = (Guid)_scenarioContext["permissionId"];

            var result = await _permissionApiClient.DeleteAsync(permissionId, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = permissionId;
        }

        private Domain.Permission CreatePermission()
        {
            var permissionId = Guid.NewGuid();
            return new Domain.Permission(
                new PermissionId(permissionId),
                new PermissionName($"Test {permissionId}"));
        }
    }
}

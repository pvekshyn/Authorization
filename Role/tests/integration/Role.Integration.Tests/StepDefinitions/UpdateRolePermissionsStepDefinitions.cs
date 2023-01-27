using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Role.Infrastructure;
using Role.SDK.DTO;
using Role.SDK.Features;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class UpdateRolePermissionsStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly RoleDbContext _dbContext;
        private IRoleApi _roleApiClient;

        public UpdateRolePermissionsStepDefinitions(
            ScenarioContext scenarioContext,
            RoleDbContext dbContext,
            IRoleApi roleApiClient)
        {
            _scenarioContext = scenarioContext;
            _dbContext = dbContext;
            _roleApiClient = roleApiClient;
        }

        [Given(@"Update payload with not existing role")]
        public void GivenUpdatePayloadWithNotExistingRole()
        {
            var roleId = Guid.NewGuid();
            var dto = GetRoleDto(roleId);
            _scenarioContext["dto"] = dto;
        }

        [When(@"Role permissions updated")]
        public async Task WhenRolePermissionsUpdated()
        {
            var dto = (UpdateRolePermissionsDto)_scenarioContext["dto"];

            var result = await _roleApiClient.UpdateRolePermissionsAsync(dto, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = dto.Id;
        }

        [Given(@"Update payload valid")]
        public void GivenUpdatePayloadValid()
        {
            var roleId = (Guid)_scenarioContext["roleId"];

            var dto = GetRoleDto(roleId);

            _scenarioContext["dto"] = dto;
        }

        [Then(@"Role permissions changed in database")]
        public async Task ThenRolePermissionsChangedInDatabase()
        {
            var dto = (UpdateRolePermissionsDto)_scenarioContext["dto"];
            var role = await _dbContext.Roles
                .Include(x => x.Permissions)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            Assert.NotNull(role);
            Assert.AreEqual(dto.PermissionIds.Single(), (Guid)role.Permissions.Single().Id);
        }

        private UpdateRolePermissionsDto GetRoleDto(Guid? roleId = null)
        {
            var newPermissionId = (Guid)_scenarioContext["newPermissionId"];
            return new UpdateRolePermissionsDto()
            {
                Id = roleId ?? Guid.NewGuid(),
                PermissionIds = new List<Guid> { newPermissionId },
            };
        }
    }
}

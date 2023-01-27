using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Role.Domain.ValueObjects.Role;
using Role.Infrastructure;
using Role.SDK.DTO;
using Role.SDK.Features;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class RenameRoleStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly RoleDbContext _dbContext;
        private IRoleApi _roleApiClient;

        public RenameRoleStepDefinitions(
            ScenarioContext scenarioContext,
            RoleDbContext dbContext,
            IRoleApi roleApiClient)
        {
            _scenarioContext = scenarioContext;
            _dbContext = dbContext;
            _roleApiClient = roleApiClient;
        }

        [Given(@"Rename payload with not existing role")]
        public void GivenRenamePayloadWithNotExistingRole()
        {
            var roleId = Guid.NewGuid();
            var dto = GetRoleDto(roleId);
            _scenarioContext["renameRoleDto"] = dto;
        }

        [Given(@"Role with this permission exist")]
        public void GivenRoleWithThisPermissionExist()
        {
            var roleId = Guid.NewGuid();

            var permissionId = (Guid)_scenarioContext["permissionId"];

            var permissions = _dbContext.Permissions
                .Where(x => x.Id == permissionId)
                .ToList();

            var role = new Domain.Role(
                new RoleId(roleId),
                new RoleName($"Test {roleId}".Substring(0, 25)),
            permissions
            );

            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();

            _scenarioContext["roleId"] = roleId;
        }

        [When(@"Role renamed")]
        public async Task WhenRoleRenamed()
        {
            var dto = (RenameRoleDto)_scenarioContext["renameRoleDto"];

            var result = await _roleApiClient.RenameRoleAsync(dto, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = dto.Id;
        }

        [Given(@"Rename payload valid")]
        public void GivenRenamePayloadValid()
        {
            var roleId = (Guid)_scenarioContext["roleId"];
            var dto = GetRoleDto(roleId);
            _scenarioContext["renameRoleDto"] = dto;
        }

        [Then(@"Role name changed in database")]
        public async Task ThenRoleNameChangedInDatabase()
        {
            var dto = (RenameRoleDto)_scenarioContext["renameRoleDto"];

            var role = await _dbContext.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            Assert.NotNull(role);
            Assert.AreEqual(dto.Name, role.Name.ToString());
        }

        private RenameRoleDto GetRoleDto(Guid? roleId = null)
        {
            return new RenameRoleDto()
            {
                Id = roleId ?? Guid.NewGuid(),
                Name = $"Test {Guid.NewGuid()}".Substring(0, 25),
            };
        }
    }
}

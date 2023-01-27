using Common.SDK;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Role.Infrastructure;
using Role.SDK.DTO;
using Role.SDK.Features;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class CreateRoleStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly RoleDbContext _dbContext;
        private IRoleApi _roleApiClient;

        public CreateRoleStepDefinitions(
            ScenarioContext scenarioContext,
            RoleDbContext dbContext,
            IRoleApi roleApiClient)
        {
            _scenarioContext = scenarioContext;
            _dbContext = dbContext;
            _roleApiClient = roleApiClient;
        }

        [Given(@"Role name empty")]
        public void GivenRoleNameEmpty()
        {
            var roleDto = GetRoleDto(string.Empty);
            _scenarioContext["roleDto"] = roleDto;
        }

        [When(@"Role created")]
        public async Task WhenRoleCreated()
        {
            var roleDto = (CreateRoleDto)_scenarioContext["roleDto"];

            var result = await _roleApiClient.CreateAsync(roleDto, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = roleDto.Id;
        }

        [Then(@"Role not in database")]
        public async Task ThenRoleNotInDatabase()
        {
            var roleId = (Guid)_scenarioContext["entityId"];

            var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
            Assert.Null(role);
        }

        [Given(@"Role valid")]
        public void GivenRoleValid()
        {
            var roleDto = GetRoleDto();
            _scenarioContext["roleDto"] = roleDto;
        }

        [Then(@"Role in database")]
        public async Task ThenRoleInDatabase()
        {
            var roleId = (Guid)_scenarioContext["entityId"];

            var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
            Assert.NotNull(role);
        }

        [Given(@"Two roles with same id and name")]
        public void GivenTwoRolesWithSameIdAndName()
        {
            var roleDto = GetRoleDto();
            var task1 = _roleApiClient.CreateAsync(roleDto, CancellationToken.None);
            var task2 = _roleApiClient.CreateAsync(roleDto, CancellationToken.None);

            _scenarioContext["tasks"] = new Task<Result>[] { task1, task2 };
        }

        [When(@"Roles created")]
        public async Task WhenRolesCreated()
        {
            var tasks = (Task<Result>[])_scenarioContext["tasks"];

            var results = await Task.WhenAll(tasks);

            _scenarioContext["results"] = results;
        }

        [Given(@"Two roles with same name")]
        public void GivenTwoRolesWithSameName()
        {
            var roleDto1 = GetRoleDto();
            var roleDto2 = GetRoleDto(roleDto1.Name);

            var task1 = _roleApiClient.CreateAsync(roleDto1, CancellationToken.None);
            var task2 = _roleApiClient.CreateAsync(roleDto2, CancellationToken.None);

            _scenarioContext["tasks"] = new Task<Result>[] { task1, task2 };
        }

        private CreateRoleDto GetRoleDto(string? name = null)
        {
            var roleId = Guid.NewGuid();
            var permissionId = (Guid)_scenarioContext["permissionId"];
            return new CreateRoleDto()
            {
                Id = roleId,
                Name = name ?? $"Test {roleId}".Substring(0, 25),
                PermissionIds = new[] { permissionId }
            };
        }
    }
}

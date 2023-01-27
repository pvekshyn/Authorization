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
    public class CreatePermissionStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly RoleDbContext _dbContext;
        private IPermissionApi _permissionApiClient;

        public CreatePermissionStepDefinitions(
            ScenarioContext scenarioContext,
            RoleDbContext dbContext,
            IPermissionApi permissionApiClient)
        {
            _scenarioContext = scenarioContext;
            _dbContext = dbContext;
            _permissionApiClient = permissionApiClient;
        }

        [Given(@"Permission name empty")]
        public void GivenPermissionNameEmpty()
        {
            var permissionDto = GetPermissionDto(string.Empty);
            _scenarioContext["permissionDto"] = permissionDto;
        }

        [Given(@"Permission valid")]
        public void GivenPermissionValid()
        {
            var permissionDto = GetPermissionDto();
            _scenarioContext["permissionDto"] = permissionDto;
        }

        [Given(@"Two permissions with same id and name")]
        public void TwoPermissionsWithSameIdAndName()
        {
            var permissionDto = GetPermissionDto();
            var task1 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            _scenarioContext["tasks"] = new Task<Result>[] { task1, task2 };
        }

        [Given(@"Two permissions with same name")]
        public void TwoPermissionsWithSameName()
        {
            var permissionDto1 = GetPermissionDto();
            var permissionDto2 = GetPermissionDto(permissionDto1.Name);
            var task1 = _permissionApiClient.CreateAsync(permissionDto1, CancellationToken.None);
            var task2 = _permissionApiClient.CreateAsync(permissionDto2, CancellationToken.None);

            _scenarioContext["tasks"] = new Task<Result>[] { task1, task2 };
        }

        [When(@"Permission created")]
        public async Task WhenPermissionCreated()
        {
            var permissionDto = (PermissionDto)_scenarioContext["permissionDto"];

            var result = await _permissionApiClient.CreateAsync(permissionDto, CancellationToken.None);

            _scenarioContext["result"] = result;
            _scenarioContext["entityId"] = permissionDto.Id;
        }

        [When(@"Permissions created")]
        public async Task WhenPermissionsCreated()
        {
            var tasks = (Task<Result>[])_scenarioContext["tasks"];

            var results = await Task.WhenAll(tasks);

            _scenarioContext["results"] = results;
        }

        [Then(@"Permission in database")]
        public async Task ThenPermissionInDatabase()
        {
            var permissionId = (Guid)_scenarioContext["entityId"];

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionId);
            Assert.NotNull(permission);
        }

        [Then(@"Permission not in database")]
        public async Task ThenPermissionNotInDatabase()
        {
            var permissionId = (Guid)_scenarioContext["entityId"];

            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionId);
            Assert.Null(permission);
        }

        private PermissionDto GetPermissionDto(string? name = null)
        {
            var permissionId = Guid.NewGuid();
            return new PermissionDto()
            {
                Id = permissionId,
                Name = name ?? $"Test {permissionId}",
            };
        }
    }
}

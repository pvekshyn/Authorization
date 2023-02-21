using Common.SpecFlowTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Refit;
using Role.Infrastructure;
using Role.SDK.Features;
using SolidToken.SpecFlow.DependencyInjection;

namespace Role.Integration.Tests
{
    [SetUpFixture]
    public class TestSetUp : TestSetUpBase
    {
        public static string ConnectionString { get; private set; }

        protected override string DbName => "Role";
        protected override IEnumerable<string> DacPacPaths => new List<string>
        {
            @"Build.Role.Database.dacpac",
            @"Build.Outbox.Database.dacpac"
        };

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            ConnectionString = await StartDatabase();
        }

        [OneTimeTearDown]
        public async Task RunAfterAnyTests()
        {
            await StopDatabase();
        }

        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<RoleDbContext>(x => x.UseSqlServer(TestSetUp.ConnectionString));

            var apiFactory = new CustomWebApplicationFactory<Program>();

            var settings = new RefitSettings
            {
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            var httpClient = apiFactory.CreateClient();
            var permissionApiClient = RestService.For<IPermissionApi>(httpClient, settings);
            var roleApiClient = RestService.For<IRoleApi>(httpClient, settings);

            services.AddSingleton(permissionApiClient);
            services.AddSingleton(roleApiClient);

            return services;
        }
    }
}

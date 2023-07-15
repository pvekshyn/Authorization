using Common.SpecFlowTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Refit;
using Assignment.Infrastructure;
using SolidToken.SpecFlow.DependencyInjection;
using Assignment.SDK.Features;
using Assignment.Host.API.Controllers;

namespace Assignment.Integration.Tests
{
    [SetUpFixture]
    public class TestSetUp : TestSetUpBase
    {
        public static string ConnectionString { get; private set; }

        protected override string DbName => "Assignment";
        protected override IEnumerable<string> DacPacPaths => new List<string>
        {
            "Build.Assignment.Database.dacpac"
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

            services.AddDbContext<AssignmentDbContext>(x => x.UseSqlServer(ConnectionString));

            var apiFactory = new CustomWebApplicationFactory<Program>();
            var httpClient = apiFactory.CreateClient();

            var settings = new RefitSettings
            {
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            var apiClient = RestService.For<IAssignmentApi>(httpClient, settings);
            services.AddSingleton(apiClient);

            var eventProcessingClient = RestService.For<IEventProcessingApi>(httpClient, settings);
            services.AddSingleton(eventProcessingClient);

            return services;
        }
    }
}

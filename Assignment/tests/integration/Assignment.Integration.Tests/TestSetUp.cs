using Common.SpecFlowTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Refit;
using Assignment.Infrastructure;
using SolidToken.SpecFlow.DependencyInjection;
using Assignment.SDK.Features;
using Grpc.Net.Client;

namespace Assignment.Integration.Tests
{
    [SetUpFixture]
    public class TestSetUp : TestSetUpBase
    {
        public static string ConnectionString { get; private set; }

        protected override string DbName => "Assignment";
        protected override IEnumerable<string> DacPacPaths => new List<string>
        {
            "Assignment.Database.dacpac",
            "Outbox.Database.dacpac"
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

            var apiClient = CreateApiClient();
            services.AddSingleton(apiClient);

            var grpcClient = CreateGrpcClient();
            services.AddSingleton(grpcClient);

            return services;
        }

        private static IAssignmentApi CreateApiClient()
        {
            var apiFactory = new CustomWebApplicationFactory<Program>();
            var httpClient = apiFactory.CreateClient();

            var settings = new RefitSettings
            {
                ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
            };

            return RestService.For<IAssignmentApi>(httpClient, settings);
        }

        private static GrpcEventProcessingService.GrpcEventProcessingServiceClient CreateGrpcClient()
        {
            var grpcFactory = new CustomWebApplicationFactory<Program>();

            var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {
                HttpClient = grpcFactory.CreateClient()
            });

            return new GrpcEventProcessingService.GrpcEventProcessingServiceClient(channel);
        }
    }
}

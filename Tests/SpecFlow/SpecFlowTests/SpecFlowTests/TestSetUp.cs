using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidToken.SpecFlow.DependencyInjection;
using SpecFlowTests.Drivers;

namespace SpecFlowTests
{
    [SetUpFixture]
    public class TestSetUp
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", optional: true)
                .Build();

            services.Configure<TestSettings>(config);

            services.AddTransient<RoleDriver>();
            services.AddTransient<AssignmentDriver>();
            services.AddTransient<AuthorizationDriver>();

            return services;
        }
    }
}

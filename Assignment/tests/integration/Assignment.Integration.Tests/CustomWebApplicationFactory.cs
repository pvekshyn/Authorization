using Assignment.Infrastructure;
using Assignment.Infrastructure.Extensions;
using Common.SpecFlowTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Integration.Tests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"SERVICE:role-grpc:HOST", "http://localhost"},
                {"SERVICE:role-grpc:PORT", "5000"},
            };

            builder.ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddInMemoryCollection(inMemorySettings);
            });

            builder.ConfigureServices(services =>
            {
                services.TurnOffAuthentication();
                services.TurnOffAuthorization();

                ReplaceDbContext(services);
            });
        }

        private static void ReplaceDbContext(IServiceCollection services)
        {
            var service = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AssignmentDbContext>));

            services.Remove(service);

            services.AddDbContext(TestSetUp.ConnectionString);
        }
    }
}

using Common.SpecFlowTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;

namespace Role.Integration.Tests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"SERVICE:authorization-grpc:HOST", "http://localhost"},
                {"SERVICE:authorization-grpc:PORT", "5000"},
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

            base.ConfigureWebHost(builder);
        }

        private static void ReplaceDbContext(IServiceCollection services)
        {
            var service = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<RoleDbContext>));

            services.Remove(service);

            services.AddDbContext(Constants.ConnectionString);
        }
    }
}

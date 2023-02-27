using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;

namespace Role.Integration.Tests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureTestServices(services =>
            {
                ReplaceDbContext(services);
            });
        }

        private static void ReplaceDbContext(IServiceCollection services)
        {
            var service = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<RoleDbContext>));

            services.Remove(service);

            services.AddDbContext(TestSetUp.ConnectionString);
        }
    }
}

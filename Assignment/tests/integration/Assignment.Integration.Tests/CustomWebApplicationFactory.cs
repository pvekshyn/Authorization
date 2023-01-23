using Assignment.Infrastructure;
using Assignment.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assignment.Integration.Tests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
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
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AssignmentDbContext>));

                services.Remove(descriptor);

                services.AddDbContext(Constants.ConnectionString);
            });
        }
    }
}

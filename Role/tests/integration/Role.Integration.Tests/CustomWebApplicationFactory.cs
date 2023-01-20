using Common.Application.Dependencies;
using Microsoft.AspNetCore.Authentication;
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
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<RoleDbContext>));

                services.Remove(descriptor);

                services.AddDbContext(Constants.ConnectionString);

                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                        "TestScheme", options => { });

                var descriptor1 = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(ICurrentUserCheckAccessService));
                services.Remove(descriptor1);
                services.AddTransient<ICurrentUserCheckAccessService, TestCurrentUserCheckAccessService>();
            });

            base.ConfigureWebHost(builder);
        }
    }
}

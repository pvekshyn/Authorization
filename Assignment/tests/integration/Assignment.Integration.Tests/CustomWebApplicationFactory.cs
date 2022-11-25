using Assignment.Infrastructure;
using Assignment.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Integration.Tests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AssignmentDbContext>));

                services.Remove(descriptor);

                services.AddDbContext("Data Source=localhost\\SQLEXPRESS;User Id=assignment_service;Password=assignment_password;Initial Catalog=Assignment;TrustServerCertificate=True");
            });
        }
    }
}

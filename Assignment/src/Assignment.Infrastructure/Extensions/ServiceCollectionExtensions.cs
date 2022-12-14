using Assignment.Application.Dependencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        return services;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        return services.AddDbContext(connectionString);
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AssignmentDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Transient);
        services.AddTransient<IAssignmentDbContext, AssignmentDbContext>();
        return services;
    }
}

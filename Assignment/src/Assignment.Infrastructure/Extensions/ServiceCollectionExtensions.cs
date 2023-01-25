using Assignment.Application.Dependencies;
using Assignment.Infrastructure.Repositories;
using Common.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAuthenticationAndAuthorization(configuration)
            .AddDbContext(configuration)
            .AddRepositories();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        return services.AddDbContext(connectionString);
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AssignmentDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Transient);
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddTransient<IAssignmentRepository, AssignmentRepository>()
            .AddTransient<IRoleRepository, RoleRepository>();
    }
}

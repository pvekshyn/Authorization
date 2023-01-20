using Common.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Role.Application.Dependencies;
using Role.Infrastructure.Repositories;

namespace Role.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCommonInfrastructureDependencies()
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
        services.AddDbContext<RoleDbContext>(x => x.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddTransient<IPermissionRepository, PermissionRepository>()
            .AddTransient<IRoleRepository, RoleRepository>();
    }
}

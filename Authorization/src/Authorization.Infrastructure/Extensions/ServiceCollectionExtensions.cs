using Authorization.Application.Dependencies;
using Authorization.Infrastructure.DataAccess.Read;
using Authorization.Infrastructure.DataAccess.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext(configuration);
    }
    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<AuthorizationDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Transient);
        services.AddTransient<IAuthorizationDbContext, AuthorizationDbContext>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddSingleton<IAssignmentRepository, AssignmentRepository>()
            .AddSingleton<IRolePermissionRepository, RolePermissionRepository>()
            .AddSingleton<IAccessRepository, AccessRepository>();
    }
}

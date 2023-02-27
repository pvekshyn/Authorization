using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Role.Application.Dependencies;
using Role.Infrastructure.Repositories;

namespace Role.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext(connectionString)
            .AddTransient<IPermissionRepository, PermissionRepository>()
            .AddTransient<IRoleRepository, RoleRepository>();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<RoleDbContext>(x => x.UseSqlServer(connectionString));
        return services;
    }

}

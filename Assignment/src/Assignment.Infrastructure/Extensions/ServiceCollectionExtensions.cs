using Assignment.Application.Dependencies;
using Assignment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext(connectionString)
            .AddTransient<IAssignmentRepository, AssignmentRepository>()
            .AddTransient<IRoleRepository, RoleRepository>();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AssignmentDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Transient);
        return services;
    }
}

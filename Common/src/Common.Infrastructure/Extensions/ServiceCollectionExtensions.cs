using Common.Application.Dependencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonInfrastructureDependencies(this IServiceCollection services)
    {
        return services.AddSingleton<ICurrentContext, CurrentContext>()
            .AddTransient<ICurrentUserCheckAccessService, CurrentUserCheckAccessService>();
    }

    public static void AddCheckAccessGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<GrpcCheckAccessService.GrpcCheckAccessServiceClient>(o =>
        {
            o.Address = GetUri(configuration, "authorization-grpc");
        });
    }

    public static Uri? GetUri(IConfiguration configuration, string serviceName)
    {
        var serviceUri = configuration.GetServiceUri(serviceName);
        if (serviceUri is null)
            throw new Exception($"Cannot get {serviceName} Service Uri");

        return serviceUri;
    }
}

using Common.Application.Dependencies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = GetUri(configuration, "identity-server").ToString();
                options.TokenValidationParameters.ValidateAudience = false;
                options.RequireHttpsMetadata = false;
            });

        services.AddHttpContextAccessor();

        services.AddSingleton<ICurrentContext, CurrentContext>()
            .AddTransient<ICurrentUserCheckAccessService, CurrentUserCheckAccessService>();

        services.AddGrpcClient<GrpcCheckAccessService.GrpcCheckAccessServiceClient>(o =>
        {
            o.Address = GetUri(configuration, "authorization-grpc");
        });

        return services;
    }

    public static Uri? GetUri(IConfiguration configuration, string serviceName)
    {
        var serviceUri = configuration.GetServiceUri(serviceName);
        if (serviceUri is null)
            throw new Exception($"Cannot get {serviceName} Service Uri");

        return serviceUri;
    }
}

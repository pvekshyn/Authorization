using Common.Application.Dependencies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Role.Integration.Tests;

namespace Common.SpecFlowTests;
public static class ServiceCollectionExtensions
{
    public static void TurnOffAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(defaultScheme: "TestScheme")
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", options => { });
    }

    public static void TurnOffAuthorization(this IServiceCollection services)
    {
        var service = services.SingleOrDefault(d => d.ServiceType == typeof(ICurrentUserCheckAccessService));
        services.Remove(service);
        services.AddTransient<ICurrentUserCheckAccessService, TestCurrentUserCheckAccessService>();
    }
}

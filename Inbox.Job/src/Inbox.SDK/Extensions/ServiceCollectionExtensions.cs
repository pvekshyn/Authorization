using Inbox.SDK.EventToRequest;
using Microsoft.Extensions.DependencyInjection;

namespace Inbox.SDK.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventToRequestMappers<TAssembly>(this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromAssemblyOf<TAssembly>()
        .AddClasses(classes => classes.AssignableTo(typeof(IEventToRequestMapper<>)))
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        services.AddTransient<IRequestFactory, RequestFactory>();

        return services;
    }
}

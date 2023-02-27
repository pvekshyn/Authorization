using Common.Application.Idempotency;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Common.Application.Behaviors;
using Common.Application.Authorization;

namespace Common.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDependencies<TAssembly>(this IServiceCollection services)
    {
        return services
            .AddValidators<TAssembly>()
            .AddIdempotencyChecks<TAssembly>()
            .AddBehaviors();
    }

    public static IServiceCollection AddValidators<TAssembly>(this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromAssemblyOf<TAssembly>()
        .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection AddAuthorizationChecks<TAssembly>(this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromAssemblyOf<TAssembly>()
        .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizationCheck<>)))
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection AddIdempotencyChecks<TAssembly>(this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromAssemblyOf<TAssembly>()
        .AddClasses(classes => classes.AssignableTo(typeof(IIdempotencyCheck<>)))
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection AddBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

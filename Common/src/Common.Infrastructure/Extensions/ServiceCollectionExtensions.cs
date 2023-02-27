using Common.Application.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection services, string identityServerUrl)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = identityServerUrl;
                options.TokenValidationParameters.ValidateAudience = false;
                options.RequireHttpsMetadata = false;
            });

        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services, string authorizationUrl)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<ICurrentContext, CurrentContext>()
            .AddTransient<ICurrentUserCheckAccessService, CurrentUserCheckAccessService>();

        services.AddGrpcClient<GrpcCheckAccessService.GrpcCheckAccessServiceClient>(o =>
        {
            o.Address = new Uri(authorizationUrl);
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithAuthentication(this IServiceCollection services, string identityServerUrl)
    {
        return services.AddSwaggerGen(c =>
        {
            var requiredScope = "api";
            var securityDefinitionId = "oath2ClientCredentials";

            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = securityDefinitionId },
                Type = SecuritySchemeType.OAuth2,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{identityServerUrl}connect/token"),
                        Scopes = new Dictionary<string, string>
                    {
                        { requiredScope, "For accessing all API" }
                    }
                    }
                }
            };

            c.AddSecurityDefinition(securityDefinitionId, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[] { requiredScope }}
            });
        });
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace Common.Infrastructure.Extensions;
public static class WebApplicationExtensions
{
    public static void UseSwaggerWithBasePath(this WebApplication app, string basePath)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
            });
        });
        app.UseSwaggerUI();

        app.UsePathBase(new PathString(basePath));
        app.UseRouting();
    }
}

using Common.Application.Extensions;
using Common.Infrastructure.Extensions;
using MediatR;
using Role.API;
using Role.API.Filters;
using Role.Application;
using Role.Host.API;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;
using Role.Infrastructure.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RoleSettings>(builder.Configuration);

var startupSettings = new StartupSettings(builder.Configuration, builder.Environment);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<StatusCodeFilter>();
});

if (startupSettings.NeedAuth())
{
    builder.Services.AddSwaggerWithAuthentication(startupSettings.IdentityServerUrl);
}
else
{
    builder.Services.AddSwaggerGen();
}

builder.Services.AddMediatR(
    typeof(IApiAssemblyMarker),
    typeof(IApplicationAssemblyMarker),
    typeof(IInfrastructureAssemblyMarker));

if (startupSettings.NeedKeyVault())
    builder.Configuration.AddKeyVault(startupSettings.KeyVaultName, startupSettings.ManagedIdentityClientId);

builder.Services.AddApplicationDependencies<IApplicationAssemblyMarker>();

if (startupSettings.NeedAuth())
{
    builder.Services.AddAuthorizationChecks<IApplicationAssemblyMarker>();
    builder.Services.AddBearerAuthentication(startupSettings.IdentityServerUrl);
    builder.Services.AddAuthorization(startupSettings.AuthorizationUrl);
}

builder.Services.AddDb(startupSettings.ConnectionString);

builder.Services.AddGrpc();

builder.Services.AddCors(p => p.AddPolicy("corsany", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCap(x =>
{
    x.UseEntityFramework<RoleDbContext>();
    x.UseRabbitMQ("localhost");
});

var app = builder.Build();

startupSettings.Log(app.Logger);

app.UseSwaggerWithBasePath("/role-api");

app.UseCors("corsany");

var cb = app.MapControllers();

if (startupSettings.NeedAuth())
{
    app.UseAuthentication();
    app.UseAuthorization();

    cb.RequireAuthorization();
}

app.MapGrpcService<RoleService>();

app.Run();

public partial class Program { }
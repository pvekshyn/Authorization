using Azure.Identity;
using Common.Application.Extensions;
using MediatR;
using Role.API;
using Role.API.Filters;
using Role.Application;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;
using Role.Infrastructure.Grpc;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<StatusCodeFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(
    typeof(IApiAssemblyMarker),
    typeof(IApplicationAssemblyMarker),
    typeof(IInfrastructureAssemblyMarker));

var keyVaultName = builder.Configuration.GetSection("KeyVaultName")?.Value;
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultEndpoint),
        new DefaultAzureCredential());
}

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<RoleSettings>(builder.Configuration);

builder.Services.AddGrpc();

var app = builder.Build();

app.Logger.LogInformation($"KV name: {keyVaultName}");
app.Logger.LogInformation(app.Configuration["SERVICE:identity-server:HOST"]);

foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
    Console.WriteLine("  {0} = {1}", de.Key, de.Value);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<RoleService>();

app.Run();

public partial class Program { }
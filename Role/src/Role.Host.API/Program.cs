using Azure.Identity;
using Common.Application.Extensions;
using MediatR;
using Role.API;
using Role.API.Filters;
using Role.Application;
using Role.Infrastructure;
using Role.Infrastructure.Extensions;

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

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<RoleSettings>(builder.Configuration);

var keyVaultEndpoint = "https://pv-role-kv.vault.azure.net";
var azureADManagedIdentityClientId = "de1a4f70-9eba-4c10-9ef6-03619e9cd42d";

var miCredentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ManagedIdentityClientId = azureADManagedIdentityClientId
});

builder.Configuration.AddAzureKeyVault(
    new Uri(keyVaultEndpoint),
    miCredentials);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
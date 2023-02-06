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
var azureADManagedIdentityClientId = "3393a89b-e05a-464f-8a7f-9850e1b811e1";

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
using Assignment.Application;
using Authorization.Infrastructure;
using Authorization.Infrastructure.DataAccess.Read;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Grpc;
using Azure.Identity;
using Common.Application.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(typeof(IApplicationAssemblyMarker), typeof(IInfrastructureAssemblyMarker));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keyVaultName = builder.Configuration.GetSection("KeyVaultName")?.Value;
if (!string.IsNullOrEmpty(keyVaultName))
{
    var managedIdentityClientId = builder.Configuration.GetSection("ManagedIdentityClientId")?.Value;
    var options = new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId };
    var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultEndpoint),
        new DefaultAzureCredential(options));
}

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>()
    .AddInfrastructureDependencies(builder.Configuration);

builder.Services.Configure<AuthorizationSettings>(builder.Configuration);

builder.Services.AddGrpc();

builder.Services.AddCap(x =>
{
    x.UseEntityFramework<AuthorizationDbContext>();
    x.UseRabbitMQ("localhost");
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGrpcService<CheckAccessService>();

app.Run();
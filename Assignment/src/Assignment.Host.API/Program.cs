using Assignment.Application;
using Assignment.Infrastructure.Extensions;
using Common.Application.Extensions;
using MediatR;
using Assignment.Infrastructure;
using Assignment.Host.API.Filters;
using Assignment.Host.API;
using Inbox.SDK.Extensions;
using Assignment.Infrastructure.Grpc;
using Inbox.SDK.Grpc;
using Azure.Identity;

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

builder.Services.AddApiApplicationDependencies<IApplicationAssemblyMarker>()
    .AddInfrastructureDependencies(builder.Configuration)
    .AddEventToRequestMappers<IInfrastructureAssemblyMarker>();

builder.Services.AddGrpc();

builder.Services.Configure<AssignmentSettings>(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<AssignmentService>();
app.MapGrpcService<EventProcessingService>();

app.Run();

public partial class Program { }
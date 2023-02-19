using Azure.Identity;
using Outbox.Job.Infrastructure.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    var settings = config.Build();
    var keyVaultName = settings.GetSection("KeyVaultName")?.Value;
    if (!string.IsNullOrEmpty(keyVaultName))
    {
        var managedIdentityClientId = settings.GetSection("ManagedIdentityClientId")?.Value;
        var options = new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId };
        var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
        config.AddAzureKeyVault(
            new Uri(keyVaultEndpoint),
            new DefaultAzureCredential(options));
    }
});

var host = builder
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();

        services.AddQuartzJob(context.Configuration)
            .AddOutboxPublisherDependencies(context.Configuration);
    })
    .Build();

await host.RunAsync();

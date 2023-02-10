using Azure.Identity;
using Outbox.Job.Infrastructure.Extensions;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    var settings = config.Build();
    var keyVaultName = settings.GetSection("KeyVaultName")?.Value;
    if (!string.IsNullOrEmpty(keyVaultName))
    {
        var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
        config.AddAzureKeyVault(
            new Uri(keyVaultEndpoint),
            new DefaultAzureCredential());
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

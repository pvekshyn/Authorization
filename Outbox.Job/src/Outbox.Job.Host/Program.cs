using Azure.Identity;
using Microsoft.Extensions.Logging.AzureAppServices;
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

builder.ConfigureLogging(logging =>
{
    logging.AddAzureWebAppDiagnostics();
});

var host = builder
    .ConfigureServices((context, services) =>
    {
        services.Configure<AzureFileLoggerOptions>(options =>
        {

            options.FileName = "azure-diagnostics-";
            options.FileSizeLimit = 50 * 1024;
            options.RetainedFileCountLimit = 5;

        });

        services.AddQuartzJob(context.Configuration)
            .AddOutboxPublisherDependencies();
    })
    .Build();

await host.RunAsync();

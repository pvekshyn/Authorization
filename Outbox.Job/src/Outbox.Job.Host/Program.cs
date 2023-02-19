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

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var configuration = host.Services.GetService<IConfiguration>();
LogConfiguration(logger, configuration);

await host.RunAsync();

void LogConfiguration(ILogger logger, IConfiguration configuration)
{
    logger.LogInformation($"KeyVaultName = {configuration.GetSection("KeyVaultName")?.Value}");
    logger.LogInformation($"ManagedIdentityClientId = {configuration.GetSection("ManagedIdentityClientId")?.Value}");
    logger.LogInformation($"DBConnectionString = {configuration.GetConnectionString("Database")}");
}

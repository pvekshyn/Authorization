using Azure.Identity;
using Inbox.Job.Infrastructure;
using Inbox.Job.Infrastructure.Extensions;

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
        services.AddQuartzJob(context.Configuration)
            .AddInboxSubscriberDependencies(context.Configuration);

        services.Configure<InboxSettings>(context.Configuration);

        services.AddGrpcClient<GrpcEventProcessingService.GrpcEventProcessingServiceClient>(o =>
        {
            o.Address = GetGrpcUri(context.Configuration);
        });
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var configuration = host.Services.GetService<IConfiguration>();
LogConfiguration(logger, configuration);

var subscriber = host.Services.GetRequiredService<IInboxSubscriber>();
await subscriber.SubscribeAsync();

await host.RunAsync();

Uri? GetGrpcUri(IConfiguration configuration)
{
    var eventProcessingServiceName = configuration["PubSub:EventProcessingServiceName"];

    var grpcServiceUri = configuration.GetServiceUri(eventProcessingServiceName, "grpc");

    return grpcServiceUri;
}

void LogConfiguration(ILogger logger, IConfiguration configuration)
{
    logger.LogInformation($"KeyVaultName = {configuration.GetSection("KeyVaultName")?.Value}");

    var inboxSettings = new InboxSettings();
    configuration.Bind(inboxSettings);

    logger.LogInformation($"EventProcessingServiceName = {inboxSettings.PubSub.EventProcessingServiceName}");
    var grpcServiceUri = GetGrpcUri(configuration);
    logger.LogInformation($"GrpcServiceUri = {grpcServiceUri}");

    foreach (var evnt in inboxSettings.PubSub.Events)
    {
        logger.LogInformation($"Event = {evnt}");
    }
}


using Inbox.Job.Infrastructure;
using Inbox.Job.Infrastructure.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddQuartzJob(context.Configuration)
            .AddInboxSubscriberDependencies();

        services.Configure<InboxSettings>(context.Configuration);

        services.AddGrpcClient<GrpcEventProcessingService.GrpcEventProcessingServiceClient>(o =>
        {
            o.Address = GetGrpcUri(context.Configuration);
        });
    })
    .Build();

var subscriber = host.Services.GetRequiredService<IInboxSubscriber>();
await subscriber.SubscribeAsync();


await host.RunAsync();

Uri? GetGrpcUri(IConfiguration configuration)
{
    var eventProcessingServiceName = configuration["EventProcessingServiceName"];
    if (string.IsNullOrEmpty(eventProcessingServiceName))
        throw new Exception("Cannot get EventProcessingServiceName from configuration");

    var grpcServiceUri = configuration.GetServiceUri(eventProcessingServiceName);
    if (grpcServiceUri is null)
        throw new Exception("Cannot get Service Uri");

    return grpcServiceUri;
}

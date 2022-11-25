using Outbox.Job.Infrastructure.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddQuartzJob(context.Configuration)
        .AddOutboxPublisherDependencies();
    })
    .Build();

await host.RunAsync();

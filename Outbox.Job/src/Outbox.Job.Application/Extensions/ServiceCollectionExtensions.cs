using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Outbox.Job.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzJob(this IServiceCollection services, IConfiguration configuration)
    {
        var jobName = configuration["JobName"];
        if (string.IsNullOrEmpty(jobName))
            throw new ArgumentNullException(nameof(jobName), "Job Name not found in configuration");

        return services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var outboxJobKey = new JobKey(jobName);

            q.AddJob<OutboxJob>(opts => opts.WithIdentity(outboxJobKey));

            q.AddTrigger(opts => opts
                .ForJob(outboxJobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1)
                .RepeatForever()));
        })
        .AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    public static IServiceCollection AddOutboxPublisherDependencies(this IServiceCollection services)
    {
        return services.AddSingleton<IOutboxPublisher, OutboxPublisherAzure>()
            .AddSingleton<IOutboxRepository, OutboxRepository>();
    }
}

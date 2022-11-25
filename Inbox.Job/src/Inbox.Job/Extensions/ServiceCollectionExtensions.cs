using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Inbox.Job.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzJob(this IServiceCollection services, IConfiguration configuration)
    {
        var jobName = configuration["JobName"];
        if (string.IsNullOrEmpty(jobName))
            throw new ArgumentNullException(nameof(jobName), "Job Name not found in configuration");

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey(jobName);

            q.AddJob<InboxJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1)
                .RepeatForever()));

        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }

    public static IServiceCollection AddInboxSubscriberDependencies(this IServiceCollection services)
    {
        return services.AddSingleton<IInboxSubscriber, InboxSubscriber>()
            .AddSingleton<IInboxRepository, InboxRepository>();
    }

}

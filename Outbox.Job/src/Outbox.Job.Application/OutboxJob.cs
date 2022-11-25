using Quartz;

namespace Outbox.Job.Infrastructure;

[DisallowConcurrentExecution]
public class OutboxJob : IJob
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IOutboxPublisher _pubSubPublisher;

    public OutboxJob(IOutboxRepository outboxRepository, IOutboxPublisher pubSubPublisher)
    {
        _outboxRepository = outboxRepository;
        _pubSubPublisher = pubSubPublisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessage = _outboxRepository.GetFirst();

        while (outboxMessage != null)
        {
            await _pubSubPublisher.PublishAsync(outboxMessage);

            outboxMessage = _outboxRepository.GetFirst();
        }
    }
}

using Microsoft.Extensions.Logging;
using Quartz;

namespace Outbox.Job.Infrastructure;

[DisallowConcurrentExecution]
public class OutboxJob : IJob
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IOutboxPublisher _pubSubPublisher;
    private readonly ILogger<OutboxJob> _logger;

    public OutboxJob(
        IOutboxRepository outboxRepository,
        IOutboxPublisher pubSubPublisher,
        ILogger<OutboxJob> logger)
    {
        _outboxRepository = outboxRepository;
        _pubSubPublisher = pubSubPublisher;
        _logger = logger;
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

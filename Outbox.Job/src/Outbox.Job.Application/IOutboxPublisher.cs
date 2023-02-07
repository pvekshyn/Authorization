using Outbox.Job.Infrastructure.Models;

namespace Outbox.Job.Infrastructure;

public interface IOutboxPublisher
{
    Task PublishAsync(OutboxMessage outboxMessage);
}

namespace Outbox.Job.Infrastructure.Models
{
    public class OutboxMessage
    {
        public Guid Id { get; init; }
        public string Message { get; init; }
    }
}

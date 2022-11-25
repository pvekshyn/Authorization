namespace Outbox.SDK.Models;
public class OutboxMessageError
{
    public Guid Id { get; init; }
    public Guid OutboxMessageId { get; init; }
    public string Message { get; init; }
    public string? StackTrace { get; init; }
    public DateTime Created { get; init; }

    public OutboxMessageError(Guid outboxMessageId, string message, string? stackTrace = null)
    {
        Id = Guid.NewGuid();
        OutboxMessageId = outboxMessageId;
        Message = message;
        StackTrace = stackTrace;
        Created = DateTime.UtcNow;
    }
}

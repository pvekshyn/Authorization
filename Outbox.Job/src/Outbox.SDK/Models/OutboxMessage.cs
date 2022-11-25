using Newtonsoft.Json;

namespace Outbox.SDK.Models;
public class OutboxMessage
{
    public Guid Id { get; init; }
    public Guid EntityId { get; init; }
    public string Message { get; init; }
    public OutboxMessageType Type { get; init; }
    public DateTime Created { get; init; }
    public ICollection<OutboxMessageError> Errors { get; private set; }

    // just for EF
    private OutboxMessage()
    {
    }

    private OutboxMessage(Guid entityId, string message, OutboxMessageType type)
    {
        Id = Guid.NewGuid();
        EntityId = entityId;
        Message = message;
        Type = type;
        Created = DateTime.UtcNow;
        Errors = new List<OutboxMessageError>();
    }
    public static OutboxMessage CreatePubSubMessage(Guid entityId, object pubSubEvent)
    {
        var jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };
        var message = JsonConvert.SerializeObject(pubSubEvent, jsonSerializerSettings);
        return new OutboxMessage(entityId, message, OutboxMessageType.PubSub);
    }

    public void AddError(string errorMessage)
    {
        var error = new OutboxMessageError(Id, errorMessage);
        Errors.Add(error);
    }

    public void AddError(Exception e)
    {
        var error = new OutboxMessageError(Id, e.Message, e.StackTrace);
        Errors.Add(error);
    }
}

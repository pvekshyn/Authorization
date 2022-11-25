using Common.SDK;

namespace Role.SDK.Events;
public class PermissionDeletedEvent : IEvent
{
    public Guid Id { get; init; }
}

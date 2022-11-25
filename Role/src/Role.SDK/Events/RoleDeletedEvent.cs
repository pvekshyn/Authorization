using Common.SDK;

namespace Role.SDK.Events;
public class RoleDeletedEvent : IEvent
{
    public Guid Id { get; init; }
}

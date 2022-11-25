using Common.SDK;
using Role.SDK.DTO;

namespace Role.SDK.Events;
public class PermissionCreatedEvent : IEvent
{
    public PermissionDto Permission { get; init; }
}

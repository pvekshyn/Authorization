using Common.SDK;
using Role.SDK.DTO;

namespace Role.SDK.Events;
public class RoleCreatedEvent : IEvent
{
    public CreateRoleDto Role { get; init; }
}

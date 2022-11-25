using Common.SDK;
using Role.SDK.DTO;

namespace Role.SDK.Events;
public class RolePermissionsChangedEvent : IEvent
{
    public UpdateRolePermissionsDto Role { get; init; }
}

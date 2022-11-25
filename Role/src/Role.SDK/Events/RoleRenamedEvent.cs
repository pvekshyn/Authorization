using Common.SDK;
using Role.SDK.DTO;

namespace Role.SDK.Events;
public class RoleRenamedEvent : IEvent
{
    public RenameRoleDto Role { get; init; }
}

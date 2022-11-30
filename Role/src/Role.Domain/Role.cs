using Common.Domain;
using Role.Domain.ValueObjects.Role;

namespace Role.Domain;

public class Role : IAggregateRoot
{
    public RoleId Id { get; private set; }
    public RoleName Name { get; private set; }

    public ICollection<Permission> Permissions { get; private set; }

    // just for EF
    private Role()
    {
    }

    public Role(RoleId id, RoleName name, ICollection<Permission> permissions)
    {
        if (permissions is null || !permissions.Any())
            throw new ArgumentException("Permissions should not be empty");

        Id = id;
        Name = name;
        Permissions = permissions;
    }

    public void Rename(RoleName name)
    {
        Name = name;
    }

    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
    }

    public void ReplacePermissions(ICollection<Permission> permissions)
    {
        if (permissions is null || !permissions.Any())
            throw new ArgumentException("Permissions should not be empty");

        Permissions = permissions;
    }
}

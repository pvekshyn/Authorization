using Role.Domain.ValueObjects.Permission;

namespace Role.Domain;

public class Permission
{
    public PermissionId Id { get; private set; }
    public PermissionName Name { get; private set; }

    //just for EF many to many
    [Obsolete]
    public ICollection<Domain.Role> Roles { get; private set; }

    // just for EF
    private Permission()
    {
    }

    public Permission(PermissionId id, PermissionName name)
    {
        Id = id;
        Name = name;
    }
}

using Common.Domain.ValueObject;

namespace Role.Domain.ValueObjects.Permission;

public class PermissionId : IdValueObject
{
    public PermissionId(Guid value) : base(value)
    {
    }

    //for inmemory ef only
    public static implicit operator Guid(PermissionId id)
    {
        return id.Value;
    }
}



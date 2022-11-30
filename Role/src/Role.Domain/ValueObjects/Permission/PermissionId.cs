using Common.Domain.ValueObject;

namespace Role.Domain.ValueObjects.Permission;

public class PermissionId : IdValueObject
{
    public PermissionId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Permission Id should not be empty");
    }

    //for inmemory ef only
    public static implicit operator Guid(PermissionId id)
    {
        return id.Value;
    }
}



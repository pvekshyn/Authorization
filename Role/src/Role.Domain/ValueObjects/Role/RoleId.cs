using Common.Domain.ValueObject;

namespace Role.Domain.ValueObjects.Role;

public class RoleId : IdValueObject
{
    public RoleId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Role Id should not be empty");
    }

    //for inmemory ef only
    public static implicit operator Guid(RoleId id)
    {
        return id.Value;
    }
}

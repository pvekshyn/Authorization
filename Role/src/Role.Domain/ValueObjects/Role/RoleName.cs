using Common.Domain.ValueObject;

namespace Role.Domain.ValueObjects.Role;

public class RoleName : StringValueObject
{
    public RoleName(string value) : base(value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Role Name should not be empty");

        if (value.Length > Constants.MaxRoleNameLength)
            throw new ArgumentException("Role Name should be short");
    }

    //for inmemory ef only
    public static implicit operator string(RoleName id)
    {
        return id.Value;
    }
}

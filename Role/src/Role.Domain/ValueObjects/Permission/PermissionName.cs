using Common.Domain.ValueObject;

namespace Role.Domain.ValueObjects.Permission;

public class PermissionName : StringValueObject
{
    public PermissionName(string value) : base(value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Permission Name should not be empty");

        if (value.Length > Constants.MaxPermissionNameLength)
            throw new ArgumentException("Permission Name should be short");
    }

    //for inmemory ef only
    public static implicit operator string(PermissionName id)
    {
        return id.Value;
    }
}

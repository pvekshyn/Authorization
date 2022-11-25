using Common.Domain.ValueObject;

namespace Assignment.Domain.ValueObjects;

public class UserId : IdValueObject
{
    public UserId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("User Id should not be empty");
    }

    //for inmemory ef only
    public static implicit operator Guid(UserId id)
    {
        return id.Value;
    }
}

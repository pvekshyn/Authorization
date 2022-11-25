using Common.Domain.ValueObject;

namespace Assignment.Domain.ValueObjects;

public class AssignmentId : IdValueObject
{
    public AssignmentId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Assignment Id should not be empty");
    }
}

namespace Common.Domain.ValueObject;

public abstract class StringValueObject : ValueObject
{
    public string Value { get; init; }
    public StringValueObject(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(StringValueObject str)
    {
        return str.Value;
    }
}

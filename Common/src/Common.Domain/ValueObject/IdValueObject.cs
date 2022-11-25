namespace Common.Domain.ValueObject
{
    public abstract class IdValueObject : ValueObject
    {
        public Guid Value { get; init; }
        public IdValueObject(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Guid(IdValueObject id)
        {
            return id.Value;
        }
    }
}

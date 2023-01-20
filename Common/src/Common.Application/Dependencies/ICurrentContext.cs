namespace Common.Application.Dependencies
{
    public interface ICurrentContext
    {
        Guid UserId { get; }
    }
}

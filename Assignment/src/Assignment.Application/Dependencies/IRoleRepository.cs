namespace Assignment.Application.Dependencies
{
    public interface IRoleRepository
    {
        Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken);
        Task CreateAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
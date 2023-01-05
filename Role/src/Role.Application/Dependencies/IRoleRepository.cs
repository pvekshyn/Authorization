namespace Role.Application.Dependencies
{
    public interface IRoleRepository
    {
        Task<Domain.Role?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Guid id, string name, CancellationToken cancellationToken);
        Task CreateAsync(Domain.Role role, CancellationToken cancellationToken);
        Task RenameAsync(Domain.Role role, CancellationToken cancellationToken);
        Task UpdatePermissionsAsync(Domain.Role role, CancellationToken cancellationToken);
        Task DeleteAsync(Domain.Role role, CancellationToken cancellationToken);
    }
}
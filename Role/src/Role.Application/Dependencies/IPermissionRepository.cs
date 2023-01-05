using Role.Domain;

namespace Role.Application.Dependencies
{
    public interface IPermissionRepository
    {
        Task<ICollection<Permission>> GetAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AnyAsync(string name, CancellationToken cancellationToken);
        Task<bool> IsLinkedToAnyRole(Guid id, CancellationToken cancellationToken);
        Task CreateAsync(Permission permission, CancellationToken cancellationToken);
        Task DeleteAsync(Guid permissionId, CancellationToken cancellationToken);
    }
}
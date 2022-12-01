using Common.SDK;
using Refit;
using Role.SDK.DTO;

namespace Role.SDK.Features
{
    public interface IPermissionApi
    {
        [Post("/permission")]
        Task<Result> CreateAsync(PermissionDto PermissionDto, CancellationToken cancellationToken);

        [Delete("/permission/{id}")]
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}

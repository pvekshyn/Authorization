using Common.SDK;
using Refit;
using Role.SDK.DTO;

namespace Role.SDK.Features
{
    public interface IReadPermissionApi
    {
        [Get("/permission/{id}")]
        Task<Result<PermissionDto>> GetPermissionAsync(Guid id);
    }
}

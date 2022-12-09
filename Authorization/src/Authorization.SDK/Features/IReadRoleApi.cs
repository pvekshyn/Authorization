using Common.SDK;
using Refit;
using Role.SDK.DTO;

namespace Role.SDK.Features
{
    public interface IReadRoleApi
    {
        [Get("/role/{id}")]
        Task<Result<RoleDto>> GetRoleAsync(Guid id);
    }
}

using Common.SDK;
using Refit;
using Role.SDK.DTO;

namespace Role.SDK.Features
{
    public interface IRoleApi
    {
        [Post("/role")]
        Task<Result> CreateAsync(CreateRoleDto roleDto, CancellationToken cancellationToken);

        [Put("/role/permissions")]
        Task<Result> UpdateRolePermissionsAsync(UpdateRolePermissionsDto dto, CancellationToken cancellationToken);

        [Put("/role/name")]
        Task<Result> RenameRoleAsync(RenameRoleDto dto, CancellationToken cancellationToken);

        [Delete("/role/{id}")]
        Task<Result> DeleteRoleAsync(Guid id, CancellationToken cancellationToken);
    }
}

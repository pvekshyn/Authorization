using Common.SDK;
using Refit;

namespace Role.SDK.Features
{
    public interface ICheckAccessApi
    {
        [Get("/checkaccess/userId/{userId}/permissionId/{permissionId}")]
        Task<bool> CheckAccessAsync(Guid userId, Guid permissionId);
    }
}

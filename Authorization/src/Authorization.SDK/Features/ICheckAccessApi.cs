using Common.SDK;
using Refit;

namespace Role.SDK.Features
{
    public interface ICheckAccessApi
    {
        [Get("/checkaccess/userId/{userId}/permissionId/{permissionId}")]
        bool CheckAccessAsync(Guid userId, Guid permissionId);
    }
}

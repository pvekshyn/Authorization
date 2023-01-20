using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Permission.Delete;

public class DeletePermissionAuthorizationCheck : IAuthorizationCheck<DeletePermission>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public DeletePermissionAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.DeletePermission);
    }
}

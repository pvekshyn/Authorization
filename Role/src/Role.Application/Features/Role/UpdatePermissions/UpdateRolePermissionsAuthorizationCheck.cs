using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Role.UpdatePermissions;

public class UpdateRolePermissionsAuthorizationCheck : IAuthorizationCheck<UpdateRolePermissions>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public UpdateRolePermissionsAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.EditRole);
    }
}

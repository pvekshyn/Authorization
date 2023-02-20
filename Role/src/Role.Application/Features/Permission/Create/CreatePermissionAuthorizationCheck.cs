using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Permission.Create;

public class CreatePermissionAuthorizationCheck : IAuthorizationCheck<CreatePermission>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public CreatePermissionAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.CreatePermission);
    }
}

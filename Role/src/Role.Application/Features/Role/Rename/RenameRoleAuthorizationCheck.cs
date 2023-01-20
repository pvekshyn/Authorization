using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Role.Rename;

public class RenameRoleAuthorizationCheck : IAuthorizationCheck<RenameRole>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public RenameRoleAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.EditRole);
    }
}

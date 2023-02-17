/*
using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Role.Delete;

public class DeleteRoleAuthorizationCheck : IAuthorizationCheck<DeleteRole>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public DeleteRoleAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.DeleteRole);
    }
}
*/

/*
using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Role.Application.Features.Role.Create;

public class CreateRoleAuthorizationCheck : IAuthorizationCheck<CreateRole>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public CreateRoleAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AuthorizationConstants.Permissions.CreateRole);
    }
}
*/

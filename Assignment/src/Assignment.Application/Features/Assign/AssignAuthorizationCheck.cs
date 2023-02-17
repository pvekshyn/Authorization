/*
using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Assignment.Application.Features.Assign;

public class AssignAuthorizationCheck : IAuthorizationCheck<Assign>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public AssignAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AssignmentConstants.Permissions.Assign);
    }
}
*/

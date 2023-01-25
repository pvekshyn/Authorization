using Common.Application.Authorization;
using Common.Application.Dependencies;

namespace Assignment.Application.Features.Deassign;

public class DeassignAuthorizationCheck : IAuthorizationCheck<Deassign>
{
    private readonly ICurrentUserCheckAccessService _currentUserCheckAccessService;

    public DeassignAuthorizationCheck(ICurrentUserCheckAccessService currentUserCheckAccessService)
    {
        _currentUserCheckAccessService = currentUserCheckAccessService;
    }

    public async Task<bool> CheckAccessAsync()
    {
        return await _currentUserCheckAccessService.CheckAccessAsync(AssignmentConstants.Permissions.Deassign);
    }
}

using Microsoft.AspNetCore.Mvc;
using Authorization.Infrastructure.DataAccess.Read;
using Role.SDK.Features;

namespace Authorization.Host.API.Controllers;

[ApiController]
public class CheckAccessController : ControllerBase, ICheckAccessApi
{
    IAccessRepository _repository;

    public CheckAccessController(IAccessRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("/checkaccess/userId/{userId}/permissionId/{permissionId}")]
    public bool CheckAccessAsync(Guid userId, Guid permissionId)
    {
        return _repository.CheckAccess(userId, permissionId);
    }
}

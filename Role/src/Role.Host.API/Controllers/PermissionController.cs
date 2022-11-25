using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.SDK;
using Role.SDK.DTO;
using Role.Application.Features.Permission.CreatePermission;
using Role.Application.Features.Permission.DeletePermission;

namespace Role.API.Controllers;

[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionController> _logger;

    public PermissionController(IMediator mediator, ILogger<PermissionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/permission")]
    public async Task<Result> CreateAsync(PermissionDto PermissionDto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreatePermission(PermissionDto), cancellationToken);
    }

    [HttpDelete("/permission")]
    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeletePermission(id), cancellationToken);
    }
}

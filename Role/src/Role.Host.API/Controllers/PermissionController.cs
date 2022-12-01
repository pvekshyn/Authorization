using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.SDK;
using Role.SDK.DTO;
using Role.Application.Features.Permission.Create;
using Role.Application.Features.Permission.DeletePermission;
using Role.SDK.Features;

namespace Role.Host.API.Controllers;

[ApiController]
public class PermissionController : ControllerBase, IPermissionApi
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

    [HttpDelete("/permission/{id}")]
    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeletePermission(id), cancellationToken);
    }
}

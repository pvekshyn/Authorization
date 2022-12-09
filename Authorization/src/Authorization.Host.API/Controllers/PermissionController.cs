using Microsoft.AspNetCore.Mvc;
using Role.SDK.DTO;
using Common.SDK;
using Role.SDK.Features;
using MediatR;
using Authorization.Application.Features.Permission;

namespace Authorization.Host.API.Controllers;

[ApiController]
public class PermissionController : ControllerBase, IReadPermissionApi
{
    private readonly IMediator _mediator;

    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/permission/{id}")]
    public async Task<Result<PermissionDto>> GetPermissionAsync(Guid id)
    {
        return await _mediator.Send(new GetPermission(id));
    }
}

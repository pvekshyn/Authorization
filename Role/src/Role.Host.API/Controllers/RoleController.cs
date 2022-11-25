using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Role.Application.Features.Role.CreateRole;
using Role.Application.Features.Role.UpdateRolePermissions;
using Role.Application.Features.Role.RenameRole;
using Role.Application.Features.Role.DeleteRole;

namespace Role.API.Controllers;

[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IMediator mediator, ILogger<RoleController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/role")]
    public async Task<Result> CreateAsync(CreateRoleDto roleDto)
    {
        return await _mediator.Send(new CreateRole(roleDto));
    }

    [HttpPut("/role/permissions")]
    public async Task<Result> UpdateRolePermissionsAsync(UpdateRolePermissionsDto dto)
    {
        return await _mediator.Send(new UpdateRolePermissions(dto));
    }

    [HttpPut("/role/name")]
    public async Task<Result> RenameRoleAsync(RenameRoleDto dto)
    {
        return await _mediator.Send(new RenameRole(dto));
    }

    [HttpDelete("/role")]
    public async Task<Result> DeleteRoleAsync(Guid id)
    {
        return await _mediator.Send(new DeleteRole(id));
    }
}

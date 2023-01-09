using Common.SDK;
using Role.SDK.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Role.Application.Features.Role.Create;
using Role.Application.Features.Role.UpdatePermissions;
using Role.Application.Features.Role.Rename;
using Role.SDK.Features;
using Role.Application.Features.Role.Delete;
using Microsoft.AspNetCore.Authorization;

namespace Role.Host.API.Controllers;

[ApiController]
[Authorize]
public class RoleController : ControllerBase, IRoleApi
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/role")]
    public async Task<Result> CreateAsync(CreateRoleDto roleDto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateRole(roleDto), cancellationToken);
    }

    [HttpPut("/role/permissions")]
    public async Task<Result> UpdateRolePermissionsAsync(UpdateRolePermissionsDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new UpdateRolePermissions(dto), cancellationToken);
    }

    [HttpPut("/role/name")]
    public async Task<Result> RenameRoleAsync(RenameRoleDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new RenameRole(dto), cancellationToken);
    }

    [HttpDelete("/role/{id}")]
    public async Task<Result> DeleteRoleAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new DeleteRole(id), cancellationToken);
    }
}

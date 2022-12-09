using Microsoft.AspNetCore.Mvc;
using Role.SDK.DTO;
using Common.SDK;
using Role.SDK.Features;
using MediatR;
using Authorization.Application.Features.Role;

namespace Authorization.Host.API.Controllers;

[ApiController]
public class RoleController : ControllerBase, IReadRoleApi
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/role/{id}")]
    public async Task<Result<RoleDto>> GetRoleAsync(Guid id)
    {
        return await _mediator.Send(new GetRole(id));
    }
}

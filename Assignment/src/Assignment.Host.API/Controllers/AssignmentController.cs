using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.SDK;
using Assignment.SDK.DTO;
using Assignment.Application.Features.Assign;
using Assignment.Application.Features.Deassign;

namespace Assignment.Host.API.Controllers;

[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AssignmentController> _logger;

    public AssignmentController(IMediator mediator, ILogger<AssignmentController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Assign user to role.
    /// </summary>
    [HttpPost("/assign")]
    public async Task<Result> AssignAsync(AssignmentDto assignmentDto)
    {
        return await _mediator.Send(new Assign(assignmentDto));
    }

    /// <summary>
    /// Deassign user from role.
    /// </summary>
    [HttpPost("/deassign")]
    public async Task<Result> DeassignAsync(Guid userId, Guid roleId)
    {
        return await _mediator.Send(new Deassign(userId, roleId));
    }
}

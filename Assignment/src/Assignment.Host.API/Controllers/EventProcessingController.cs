using Assignment.Application.Features.Role;
using Common.SDK;
using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Role.SDK.Events;

namespace Assignment.Host.API.Controllers;

[ApiController]
public class EventProcessingController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [CapSubscribe("Role.SDK.Events.RoleCreatedEvent")]
    [HttpPost("/rolecreated")]
    public async Task<Result> RoleCreated(RoleCreatedEvent roleCreatedEvent)
    {
        return await _mediator.Send(new CreateRole(roleCreatedEvent.Role.Id));
    }

    [CapSubscribe("Role.SDK.Events.RoleDeletedEvent")]
    [HttpPost("/roledeleted")]
    public async Task RoleDeleted(RoleDeletedEvent roleDeletedEvent)
    {
        await _mediator.Send(new DeleteRole(roleDeletedEvent.Id));
    }
}

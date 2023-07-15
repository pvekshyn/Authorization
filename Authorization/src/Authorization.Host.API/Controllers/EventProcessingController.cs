using Assignment.SDK.Events;
using Authorization.Application.Features.Assignment;
using Authorization.Application.Features.Assignment.Assign;
using Authorization.Application.Features.Permission;
using Authorization.Application.Features.Role;
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

    [CapSubscribe("Role.SDK.Events.PermissionCreatedEvent")]
    [HttpPost("/permissioncreated")]
    public async Task<Result> PermissionCreated(PermissionCreatedEvent permissionCreatedEvent)
    {
        return await _mediator.Send(new CreatePermission(permissionCreatedEvent.Permission));
    }

    [CapSubscribe("Role.SDK.Events.PermissionDeletedEvent")]
    [HttpPost("/permissiondeleted")]
    public async Task<Result> PermissionDeleted(PermissionDeletedEvent permissionDeletedEvent)
    {
        return await _mediator.Send(new DeletePermission(permissionDeletedEvent.Id));
    }

    [CapSubscribe("Role.SDK.Events.RoleCreatedEvent")]
    [HttpPost("/rolecreated")]
    public async Task<Result> RoleCreated(RoleCreatedEvent roleCreatedEvent)
    {
        return await _mediator.Send(new CreateRole(roleCreatedEvent.Role));
    }

    [CapSubscribe("Role.SDK.Events.RoleRenamedEvent")]
    [HttpPost("/rolerenamed")]
    public async Task<Result> RoleRenamed(RoleRenamedEvent roleRenamedEvent)
    {
        return await _mediator.Send(new RenameRole(roleRenamedEvent.Role));
    }

    [CapSubscribe("Role.SDK.Events.RolePermissionsChangedEvent")]
    [HttpPost("/rolepermissionschanged")]
    public async Task<Result> RolePermissionsChanged(RolePermissionsChangedEvent rolePermissionsChangedEvent)
    {
        return await _mediator.Send(new UpdateRolePermissions(rolePermissionsChangedEvent.Role));
    }

    [CapSubscribe("Role.SDK.Events.RoleDeletedEvent")]
    [HttpPost("/roledeleted")]
    public async Task RoleDeleted(RoleDeletedEvent roleDeletedEvent)
    {
        await _mediator.Send(new DeleteRole(roleDeletedEvent.Id));
    }

    [CapSubscribe("Assignment.SDK.Events.AssignmentEvent")]
    [HttpPost("/assignment")]
    public async Task Assignment(AssignmentEvent assignmentEvent)
    {
        await _mediator.Send(new Assign(assignmentEvent.Assignment));
    }

    [CapSubscribe("Assignment.SDK.Events.DeassignmentEvent")]
    [HttpPost("/deassignment")]
    public async Task Deassignment(DeassignmentEvent deassignmentEvent)
    {
        await _mediator.Send(new Deassign(deassignmentEvent.Assignment.Id));
    }
}

using Authorization.Application.Features.Role;
using Common.SDK;
using Inbox.SDK.EventToRequest;
using MediatR;
using Role.SDK.Events;

namespace Authorization.Infrastructure.PubSub.EventToRequestMappers.Role
{
    public class RolePermissionsUpdatedEventMapper : EventToRequestMapper<RolePermissionsChangedEvent>
    {
        public override IRequest<Result> Map(RolePermissionsChangedEvent @event)
        {
            return new UpdateRolePermissions(@event.Role);
        }
    }
}
